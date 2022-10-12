using PhotoManager.Core.Interfaces.Repository;
using PhotoManager.Core.Models;
using PhotoManager.Data;
using System.Collections.Generic;

namespace PhotoManager.Infrastructure.Repository
{
    public class PhotoRepository : Repository<Photo>, IPhotoRepository
    {
        private readonly ApplicationContext _context;
        public PhotoRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
        public void DeleteRange(IEnumerable<Photo> photos)
        {
            _context.Photos.RemoveRange(photos);
        }
    }
}