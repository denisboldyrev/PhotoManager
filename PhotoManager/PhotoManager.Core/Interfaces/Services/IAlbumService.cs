using PhotoManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoManager.Core.Interfaces.Services
{
    public interface IAlbumService
    {
        public Task<IReadOnlyList<Album>> GetAllAlbumsOfUserAsync(Guid userId);
        public Task<Album> GetAlbumAsync(Guid userId, Guid albumId);
        public Task<Album> CreateAlbumAsync(Album album);
        public Task DeleteAlbumAsync(Album album);
        public Task<Album> EditAlbumAsync(Album albumDto);
        public Task<Album> GetAlbumByTitleAsync(string title);
        public Task<IReadOnlyList<Photo>> GetAllPhotosFromAlbumAsync(Guid albumId);
        public Task AddPhotosToAlbumsAsync(Guid userId, List<Guid> albumsId, List<Guid> photosId);
        public Task AddPhotoToAlbumAsync(Guid albumId, Guid photoId);
        public Task<Album> DeletePhotosFromAlbumAsync(Guid userId, Guid albumId, List<Guid> photosId);
        public Task DeletePhotoFromAlbumAsync(Guid albumId, Guid photoId);
        public bool FindRating(Guid userId, Guid photoId);
    }
}
