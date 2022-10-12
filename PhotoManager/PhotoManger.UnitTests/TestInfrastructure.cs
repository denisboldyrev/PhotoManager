using Moq;
using PhotoManager.Core.Interfaces.Services;
using PhotoManager.Infrastructure.Repository;

namespace PhotoManger.UnitTests
{
    public class TestInfrastructure
    {
        private readonly UnitOfWork _unitOfWork;

        public TestInfrastructure()
        {
            _unitOfWork = MockUnitOfWork().Object;
        }
        
        public IAlbumService InitAlbumService()
        {
            var albumService = MockAlbumService();
            return albumService.Object;
        }

        public IPhotoService InitPhotoService()
        {
            var photoService = MockPhotoService();
            return photoService.Object;
        }
        #region Private Methods
        private Mock<UnitOfWork> MockUnitOfWork()
        {
            var unitOfWork = new Mock<UnitOfWork>(null);
            return unitOfWork;
        }
        private Mock<IAlbumService> MockAlbumService()
        {
            var albumService = new Mock<IAlbumService>(null);
            return albumService;
        }
        private Mock<IPhotoService> MockPhotoService()
        {
            var fileService = MockFileService().Object;
            var photoService = new Mock<IPhotoService>(null, null, null);
            return photoService;
        }
        private Mock<IFileService> MockFileService()
        {
            var fileService = new Mock<IFileService>();
            return fileService;
        }
    }
    #endregion
}
