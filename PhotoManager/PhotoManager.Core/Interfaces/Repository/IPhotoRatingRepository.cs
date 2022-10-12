using PhotoManager.Core.Models;
using System;
using System.Threading.Tasks;

namespace PhotoManager.Core.Interfaces.Repository
{
    public interface IPhotoRatingRepository
    {
        Task Like(Guid userId, Guid photoId);
        PhotoRating FindRating(Guid userId, Guid photoId);
    }
}
