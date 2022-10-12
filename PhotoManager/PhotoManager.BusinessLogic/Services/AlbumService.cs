using PhotoManager.Core.Interfaces.Repository;
using PhotoManager.Core.Interfaces.Services;
using PhotoManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoManager.BusinessLogic
{
    public class AlbumService : IAlbumService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AlbumService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<Album>> GetAllAlbumsOfUserAsync(Guid userId)
        {
            var albums = await _unitOfWork.Albums.GetAllAsync(u => u.UserId == userId);
            return albums;
        }

        public async Task<Album> GetAlbumAsync(Guid userId, Guid albumId)
        {
            var album = await _unitOfWork.Albums.GetByIdAsync(albumId);
            if (album != null && album.UserId == userId)
                return album;
            return null;
        }

        public async Task<Album> CreateAlbumAsync(Album album)
        {
            if (album == null)
                throw new ArgumentNullException(nameof(album));
            await _unitOfWork.Albums.AddAsync(album);
            await _unitOfWork.SaveAsync();
            return album;
        }

        public async Task DeleteAlbumAsync(Album album)
        {
            if (album == null)
                throw new ArgumentNullException(nameof(album));
            _unitOfWork.Albums.Delete(album);
            await _unitOfWork.SaveAsync();
        }

        public async Task<Album> EditAlbumAsync(Album album)
        {
            if (album == null)
                throw new ArgumentNullException(nameof(album));
            var albumToUpdate = await _unitOfWork.Albums.GetByIdAsync(album.Id);
            if (albumToUpdate == null)
                throw new NullReferenceException(nameof(albumToUpdate));
            albumToUpdate.Title = album.Title;
            albumToUpdate.Description = album.Description;
            await _unitOfWork.SaveAsync();

            return albumToUpdate;
        }

        public async Task<Album> GetAlbumByTitleAsync(string title)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentNullException(nameof(title));
            var album = await _unitOfWork.Albums.FindByNameAsync(title);
            return album;
        }

        public async Task<IReadOnlyList<Photo>> GetAllPhotosFromAlbumAsync(Guid albumId)
        {
            var album = await _unitOfWork.Albums.GetByIdAsync(albumId);
            if (album == null)
                throw new NullReferenceException(nameof(album));
            return album.Photos.ToList();
        }
        public async Task AddPhotoToAlbumAsync(Guid albumId, Guid photoId)
        {
            var album = await _unitOfWork.Albums.GetByIdAsync(albumId);
            if (album == null)
                throw new NullReferenceException(nameof(album));
            var photo = await _unitOfWork.Photos.GetByIdAsync(photoId);
            album.Photos.Add(photo);
            await _unitOfWork.SaveAsync();
        }
        public async Task AddPhotosToAlbumsAsync(Guid userId, List<Guid> albumsId, List<Guid> photosId)
        {
            var albums = await _unitOfWork.Albums.GetAllAsync(a => albumsId.Contains(a.Id) && a.UserId == userId);
            var photos = await _unitOfWork.Photos.GetAllAsync(p => photosId.Contains(p.Id) && p.UserId == userId);
            foreach (var album in albums)
            {
                var ph = photos.Except(album.Photos);
                foreach (var p in ph)
                {
                    album.Photos.Add(p);
                }
            }
            await _unitOfWork.SaveAsync();
        }
        public async Task DeletePhotoFromAlbumAsync(Guid albumId, Guid photoId)
        {
            var album = await _unitOfWork.Albums.GetByIdAsync(albumId)
                ?? throw new NullReferenceException();

            var photo = album.Photos.FirstOrDefault(p => p.Id == photoId);
            album.Photos.Remove(photo);
            await _unitOfWork.SaveAsync();
        }
        public bool FindRating(Guid userId, Guid photoId)
        {
            var rate = _unitOfWork.PhotoRatings.FindRating(userId, photoId);
            if (rate != null)
                return true;
            return false;
        }
        public async Task<Album> DeletePhotosFromAlbumAsync(Guid userId, Guid albumId, List<Guid> photosId)
        {
            var album = await _unitOfWork.Albums.GetByIdAsync(albumId)
                ?? throw new NullReferenceException();

            var photos = album.Photos.Where(p => photosId.Contains(p.Id)).ToList();
            foreach (var p in photos)
            {
                album.Photos.Remove(p);
            }
            await _unitOfWork.SaveAsync();
            return album;
        }
    }
}
