using PhotoManager.Core.Interfaces.Repository;
using PhotoManager.Core.Models;
using PhotoManager.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoManager.Infrastructure.Repository
{
    public class PhotoRatingRepository : IPhotoRatingRepository
    {
        private readonly ApplicationContext _context;
        public PhotoRatingRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task Like(Guid userId, Guid photoId)
        {
            var rating = new PhotoRating { PhotoId = photoId, UserId = userId, Rating = true };
            var findRating = FindRating(userId, photoId);
            if (findRating == null)
            {
                await _context.PhotoRatings.AddAsync(rating);
                await _context.SaveChangesAsync();
                return;
            }

            _context.PhotoRatings.Remove(findRating);
            await _context.SaveChangesAsync();
        }

        public PhotoRating FindRating(Guid userId, Guid photoId)
        {
            var rate = _context.PhotoRatings.SingleOrDefault(r => r.UserId == userId && r.PhotoId == photoId);
            return rate;
        }
    }
}