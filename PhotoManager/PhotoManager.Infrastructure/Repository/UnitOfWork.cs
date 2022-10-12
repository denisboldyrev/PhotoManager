using PhotoManager.Core.Interfaces.Repository;
using PhotoManager.Data;
using System;
using System.Threading.Tasks;

namespace PhotoManager.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IPhotoRepository _photos;
        private IAlbumRepository _albums;
        private IPhotoRatingRepository _photoRatings;

        private readonly ApplicationContext _context;
        private bool _disposed = false;
        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
        }
        public IPhotoRepository Photos
        {
            get
            {
                if (_photos == null)
                {
                    _photos = new PhotoRepository(_context);
                }
                return _photos;
            }
        }
        public IAlbumRepository Albums
        {
            get
            {
                if (_albums == null)
                {
                    _albums = new AlbumRepository(_context);
                }
                return _albums;
            }
        }
        public IPhotoRatingRepository PhotoRatings
        {
            get
            {
                if (_photoRatings == null)
                {
                    _photoRatings = new PhotoRatingRepository(_context);
                }
                return _photoRatings;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}