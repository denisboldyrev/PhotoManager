using FluentAssertions;
using Moq;
using PhotoManager.BusinessLogic;
using PhotoManager.Core.Interfaces.Repository;
using PhotoManager.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace PhotoManger.UnitTests
{
    public class AlbumServiceTests
    {
        private readonly Guid _userId = Guid.Parse("A777BDE3-68DA-48D3-B0BD-38C880CF0AF8");
        private readonly Guid _albumId = Guid.Parse("DE81342B-B2F6-4DC4-1F8D-08D958DCB324");
        private readonly Guid _photoId = Guid.Parse("8A033F96-17BC-45DD-7FFF-08D95BCE6D71");

        private readonly Mock<IAlbumRepository> _mockAlbumRepo;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public AlbumServiceTests()
        {
            //var options = new DbContextOptionsBuilder<ApplicationContext>()
            //         .UseInMemoryDatabase(databaseName: "MockDB")
            //         .Options;

            //var context = new ApplicationContext(options);
            _mockAlbumRepo = new Mock<IAlbumRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
        }

        public class AlbumsTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { null };
                yield return new object[] { new Album { Id = Guid.NewGuid(), Title = null } };
                yield return new object[] { new Album { Id = Guid.NewGuid(), Title = "" } };
                yield return new object[] { new Album { Id = Guid.NewGuid(), Title = " " } };
                yield return new object[] { new Album { Id = Guid.NewGuid(), Title = "  " } };
                yield return new object[] { new Album { Id = Guid.NewGuid(), Title = "   " } };
                yield return new object[] { new Album { Id = Guid.NewGuid(), Title = "Album 1" } };
                yield return new object[] { new Album { Id = Guid.NewGuid(), Title = "Album 1" } };
                yield return new object[] { new Album { Id = Guid.NewGuid(), Title = "Album 2" } };
                yield return new object[] { new Album { Id = Guid.NewGuid(), Title = "<script>alert('Album 1')</script>" } };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(AlbumsTestData))]
        public async Task CreateAlbumAsync(Album album)
        {
            _mockAlbumRepo.Setup(r => r.AddAsync(album));
            _mockUnitOfWork.Setup(u => u.Albums).Returns(_mockAlbumRepo.Object);

            var albumService = new AlbumService(_mockUnitOfWork.Object);
            await albumService.CreateAlbumAsync(album);
            var albums = await albumService.GetAllAlbumsOfUserAsync(_userId);

            _mockAlbumRepo.Verify(); 
        }
        [Fact]
        public async Task CreateAlbumWithSameTitle()
        {
            var title = "Album 1";
            var album = new Album { Id = _albumId, Title = title, UserId = _userId };

            _mockAlbumRepo.Setup(r => r.AddAsync(album));
            _mockUnitOfWork.Setup(u => u.Albums).Returns(_mockAlbumRepo.Object);

            var albumService = new AlbumService(_mockUnitOfWork.Object);
            
            await albumService.CreateAlbumAsync(album);
            await albumService.CreateAlbumAsync(album);

            var albums = albumService.GetAllAlbumsOfUserAsync(_userId);

            _mockAlbumRepo.Verify();
        }

        [Fact]
        public async Task AddPhotoToAlbum()
        {
            var expectedTitle = "Cars";
            var album = new Album { Id = _albumId, Title = expectedTitle, UserId = _userId };
            var photos = new List<Photo>
            {
                new Photo { Id = Guid.NewGuid(), Title = "Title 1", FileName = "filename", UserId = _userId },
                new Photo { Id = Guid.NewGuid(), Title = "Title 2", FileName = "filename", UserId = _userId },
                new Photo { Id = Guid.NewGuid(), Title = "Title 3", FileName = "filename", UserId = _userId },
            };


            _mockAlbumRepo.Setup(r => r.GetByIdAsync(_albumId)).ReturnsAsync(album).Verifiable();
            _mockUnitOfWork.Setup(u => u.Albums).Returns(_mockAlbumRepo.Object);
            var photosId = photos.Select(p => p.Id).ToList();
            
            var albumService = new AlbumService(_mockUnitOfWork.Object);
            await albumService.AddPhotosToAlbumsAsync(_userId, new List<Guid> { _albumId }, photosId);
            var allPhotosFromAlbum = await albumService.GetAllPhotosFromAlbumAsync(_albumId);

            allPhotosFromAlbum.Count.Should().Be(3);
        }

        [Fact]
        public async Task GetAlbumByIdAync()
        {
            var expectedTitle = "Cars";
            var album = new Album { Id = _albumId, Title = expectedTitle, UserId = _userId };

            _mockAlbumRepo.Setup(r => r.GetByIdAsync(_albumId)).ReturnsAsync(album).Verifiable();
            _mockUnitOfWork.Setup(u => u.Albums).Returns(_mockAlbumRepo.Object);

            var albumService = new AlbumService(_mockUnitOfWork.Object);
            var actual = await albumService.GetAlbumAsync(_userId, _albumId);
            _mockAlbumRepo.Verify();

            actual.Should().BeSameAs(album);
        }
        [Fact]
        public async Task GetAlbumByIdNegativeAync()
        {
            var expectedTitle = "Cars";
            var album = new Album { Id = _albumId, Title = expectedTitle, UserId = Guid.Parse("A777BDE3-68DA-48D3-B0BD-38C880CF0AF9") };

            _mockAlbumRepo.Setup(r => r.GetByIdAsync(_albumId)).ReturnsAsync(album).Verifiable();
            _mockUnitOfWork.Setup(u => u.Albums).Returns(_mockAlbumRepo.Object);

            var albumService = new AlbumService(_mockUnitOfWork.Object);
            var actual = await albumService.GetAlbumAsync(Guid.Parse("A777BDE3-68DA-48D3-B0BD-38C880CF0AF9"), Guid.Parse("A777BDE4-68DA-48D3-B0BD-38C880CF0AF8"));
            
            actual.Should().Be(null);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        [InlineData("     ")]
        [InlineData("       ")]
        [InlineData("Nature")]
        [InlineData("Animals")]
        [InlineData("Album 4")]
        [InlineData("Cars")]
        public async Task GetAlbumByTitle(string albumTitle)
        {
            var expectedTitle = "Cars";
            var album = new Album { Id = _albumId, Title = expectedTitle, UserId = Guid.Parse("A777BDE3-68DA-48D3-B0BD-38C880CF0AF9") };

            _mockAlbumRepo.Setup(r => r.FindByNameAsync(albumTitle)).ReturnsAsync(album).Verifiable();
            _mockUnitOfWork.Setup(u => u.Albums).Returns(_mockAlbumRepo.Object);

            var albumService = new AlbumService(_mockUnitOfWork.Object);
            var actual = await albumService.GetAlbumByTitleAsync(albumTitle);

            actual.Title.Should().Be(albumTitle);
        }

        [Fact]
        public async Task GetAllAlbumsOfCurrentUserAsync()
        {
            Expression<Func<Album, bool>> testExpression = binding => binding.UserId == _userId;

            var albums = new List<Album> { 
                new Album { Id = Guid.NewGuid(), Title = "Album 1", UserId = _userId },
                new Album { Id = Guid.NewGuid(), Title = "Album 2", UserId = _userId },
            };

            _mockAlbumRepo.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Album, bool>>>())).ReturnsAsync(albums);
            _mockUnitOfWork.Setup(u => u.Albums).Returns(_mockAlbumRepo.Object);

            var albumService = new AlbumService(_mockUnitOfWork.Object);
            var actual = await albumService.GetAllAlbumsOfUserAsync(_userId);

            actual.Count.Should().Be(2);
        }
        [Fact]
        public async Task DeleteAlbumAsync()
        {
            var title = "Album 1";
            var album = new Album { Id = _albumId, Title = title, UserId = _userId };
          
            _mockAlbumRepo.Setup(r => r.Delete(album)).Verifiable();
            _mockUnitOfWork.Setup(u => u.Albums).Returns(_mockAlbumRepo.Object);

            var albumService = new AlbumService(_mockUnitOfWork.Object);
            await albumService.DeleteAlbumAsync(album);

            _mockAlbumRepo.Verify();
        }
        [Fact]
        public void DeleteAlbumNegative()
        {
            var title = "Album 1";
            var album = new Album { Id = _albumId, Title = title, UserId = _userId };

            _mockAlbumRepo.Setup(r => r.Delete(album)).Verifiable();
            _mockUnitOfWork.Setup(u => u.Albums).Returns(_mockAlbumRepo.Object);

            var albumService = new AlbumService(_mockUnitOfWork.Object);
            Func<Task> act = async () => await albumService.DeleteAlbumAsync(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task EditAlbumAsync()
        {
            var title = "Album 1";
            var album = new Album { Id = _albumId, Title = title, UserId = _userId };

            _mockAlbumRepo.Setup(r => r.GetByIdAsync(_albumId)).ReturnsAsync(album).Verifiable();
            _mockAlbumRepo.Setup(r => r.Edit(It.IsAny<Album>()));
            _mockUnitOfWork.Setup(u => u.Albums).Returns(_mockAlbumRepo.Object);
            
            var albumService = new AlbumService(_mockUnitOfWork.Object);
            await albumService.CreateAlbumAsync(album);         
            album.Title = "New Title";
            var albumAfterEdit = await albumService.EditAlbumAsync(album);

            albumAfterEdit.Title.Should().Be(album.Title);
        }
    }
}