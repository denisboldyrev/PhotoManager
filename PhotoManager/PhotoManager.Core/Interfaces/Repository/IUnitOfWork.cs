using System.Threading.Tasks;

namespace PhotoManager.Core.Interfaces.Repository
{
    public interface IUnitOfWork
    {
        public IPhotoRepository Photos { get; }
        public IAlbumRepository Albums { get; }
        public IPhotoRatingRepository PhotoRatings { get; }
        public Task SaveAsync();
    }
}
