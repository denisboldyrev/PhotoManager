using PhotoManager.Core.Interfaces.Repository;
using PhotoManager.Core.Interfaces.Services;
using PhotoManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoManager.BusinessLogic
{
    public class PhotoService : IPhotoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PhotoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Photo> GetPhotoByIdAsync(Guid photoId)
        {
            var photo = await _unitOfWork.Photos.GetByIdAsync(photoId);
            return photo;
        }

        public async Task<IReadOnlyList<Photo>> GetAllPhotosOfCurrentUserAsync(Guid userId)
        {
            var photos = await _unitOfWork.Photos.GetAllAsync(p => p.UserId == userId);
            return photos;
        }

        public async Task<Photo> EditPhotoAsync(Guid userId, Photo photo)
        {
            var photoToUpdate = await _unitOfWork.Photos.GetByIdAsync(photo.Id);

            photoToUpdate.Title = photo.Title;
            photoToUpdate.DateOfTaking = photo.DateOfTaking;
            photoToUpdate.Place = photo.Place;
            photoToUpdate.CameraModel = photo.CameraModel;
            photoToUpdate.LensfocalLength = photo.LensfocalLength;
            photoToUpdate.Diaphragm = photo.Diaphragm;
            photoToUpdate.ShutterSpeed = photo.ShutterSpeed;
            photoToUpdate.ISO = photo.ISO;
            photoToUpdate.Flash = photo.Flash;

            await _unitOfWork.SaveAsync();
            return photoToUpdate;
        }

        public async Task DeleteAsync(Photo photo)
        {
            _unitOfWork.Photos.Delete(photo);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteRangeAsync(Guid userId, List<Guid> photosId)
        {
            var photos = (await _unitOfWork.Photos.GetAllAsync(p => photosId.Contains(p.Id) && p.UserId == userId)).ToArray();
            _unitOfWork.Photos.DeleteRange(photos);
            await _unitOfWork.SaveAsync();
        }

        public async Task UploadAsync(Guid userId, Photo photo)
        {
            photo.UploadDT = DateTime.Now;

            if (photo.UserId != userId)
                photo.UserId = userId;

            await _unitOfWork.Photos.AddAsync(photo);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IReadOnlyList<Photo>> SearchPhotosAsync(Guid userId, string searchString)
        {
            var photos = await GetAllPhotosOfCurrentUserAsync(userId);

            if (string.IsNullOrEmpty(searchString))
                return photos;

            var splitSearchString = searchString.Trim().Split(" ").Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim().ToLowerInvariant()).ToList();

            var filteredPhotos = photos.Where(p =>
                    !string.IsNullOrWhiteSpace(p.Title) && splitSearchString.Any(p.Title.ToLower().Contains) ||
                    !string.IsNullOrWhiteSpace(p.DateOfTaking) && splitSearchString.Any(p.DateOfTaking.ToLower().Contains) ||
                    !string.IsNullOrWhiteSpace(p.Place) && splitSearchString.Any(p.Place.ToLower().Contains) ||
                    !string.IsNullOrWhiteSpace(p.CameraModel) && splitSearchString.Any(p.CameraModel.ToLower().Contains) ||
                    !string.IsNullOrWhiteSpace(p.LensfocalLength) && splitSearchString.Any(p.LensfocalLength.ToLower().Contains) ||
                    !string.IsNullOrWhiteSpace(p.Diaphragm) && splitSearchString.Any(p.Diaphragm.ToLower().Contains) ||
                    splitSearchString.Any(p.ISO.ToString().Contains) ||
                    splitSearchString.Any(p.Flash.ToString().Contains));

            return filteredPhotos.ToList();
        }

        public async Task LikeAsync(Guid userId, Guid photoId)
        {
            await _unitOfWork.PhotoRatings.Like(userId, photoId);
            await _unitOfWork.SaveAsync();
        }
    }
}
