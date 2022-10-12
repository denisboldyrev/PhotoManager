using PhotoManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoManager.Core.Interfaces.Services
{
    public interface IPhotoService
    {
        public Task<Photo> GetPhotoByIdAsync(Guid photoId);
        public Task<IReadOnlyList<Photo>> GetAllPhotosOfCurrentUserAsync(Guid userId);
        public Task<Photo> EditPhotoAsync(Guid userId, Photo photo);
        public Task DeleteRangeAsync(Guid userId, List<Guid> photosId);
        public Task DeleteAsync(Photo photo);
        public Task UploadAsync(Guid userId, Photo photo);
        public Task<IReadOnlyList<Photo>> SearchPhotosAsync(Guid userId, string searchString);
        public Task LikeAsync(Guid userId, Guid photoId);
    }
}
