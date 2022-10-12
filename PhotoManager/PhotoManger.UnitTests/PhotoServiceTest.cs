using FluentAssertions;
using Moq;
using PhotoManager.BusinessLogic;
using PhotoManager.Core.Interfaces.Repository;
using PhotoManager.Core.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PhotoManger.UnitTests
{
    public class PhotoServiceTest
    {
        private readonly Guid _userId = Guid.Parse("A777BDE3-68DA-48D3-B0BD-38C880CF0AF8");
        private readonly Guid _albumId = Guid.Parse("DE81342B-B2F6-4DC4-1F8D-08D958DCB324");
        private readonly Guid _photoId = Guid.Parse("8A033F96-17BC-45DD-7FFF-08D95BCE6D71");
        private readonly Mock<IAlbumRepository> _mockAlbumRepo;
        private readonly Mock<IPhotoRepository> _mockPhotoRepo;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public PhotoServiceTest()
        {
            //var options = new DbContextOptionsBuilder<ApplicationContext>()
            //         .UseInMemoryDatabase(databaseName: "MockDB")
            //         .Options;

            //var context = new ApplicationContext(options);
            _mockAlbumRepo = new Mock<IAlbumRepository>();
            _mockPhotoRepo = new Mock<IPhotoRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async Task GetPhotoById()
        {
            var expectedTitle = "Photo 1";
            var photo = new Photo { Id = _photoId, Title = expectedTitle, FileName="FileName", Size = 3, UserId = _userId };

            _mockPhotoRepo.Setup(r => r.GetByIdAsync(_photoId)).ReturnsAsync(photo);
            _mockUnitOfWork.Setup(u => u.Photos).Returns(_mockPhotoRepo.Object);

            var photoService = new PhotoService(_mockUnitOfWork.Object);
            var actual = await photoService.GetPhotoByIdAsync(_photoId);

            actual.Should().BeSameAs(photo);
        }
    }
}
