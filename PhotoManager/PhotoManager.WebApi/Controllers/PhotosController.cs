using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PhotoManager.BusinessLogic;
using PhotoManager.Core.Interfaces.Services;
using PhotoManager.Core.Models;
using PhotoManager.WebApi.Extensions;
using PhotoManager.WebApi.Filters;
using PhotoManager.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoManager.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IPhotoService _photoService;
        private readonly IConfiguration _config;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public PhotosController(IPhotoService photoService, IConfiguration config, IFileService fileService, IMapper mapper)
        {
            _photoService = photoService;
            _fileService = fileService;
            _config = config;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all photos of current user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/photos")]
        public async Task<IActionResult> GetPhotos()
        {
            var photos = await _photoService.GetAllPhotosOfCurrentUserAsync(this.GetUserId());
            var photosDto = _mapper.Map<IReadOnlyList<PhotoDto>>(photos);
            return Ok(photosDto);
        }

        /// <summary>
        /// Upload new photo
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/photos")]
        [ServiceFilter(typeof(ValidateImgExtAndSizeAttribute))]
        [ServiceFilter(typeof(ValidateUsersPermitionsAttribute<Photo>))]
        public async Task<IActionResult> UploadPhoto()
        {
            var formFile =Request.Form.Files[0];
            var uniqueFileName = ImageProcessor.GetUniqueFileName(formFile.FileName);
            var originalPath = _config["OriginalImgPath"] + uniqueFileName;
            var thumbnailPath = _config["ThumbnailImgPath"] + uniqueFileName;
            var midsizePath = _config["MidSizeImgPath"] + uniqueFileName;

            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(errorMessage);
            }
            var image = Image.FromStream(formFile.OpenReadStream());
            var exifReader = new ExifReader(image);
            var photo = new Photo
            {
                FileName = uniqueFileName,
                Size = formFile.Length,
                Title = exifReader.GetTitle(),
                DateOfTaking = exifReader.GetDate(),
                Place = "",
                CameraModel = exifReader.GetCameraModel(),
                LensfocalLength = exifReader.GetFocalLength(),
                Diaphragm = exifReader.GetDiaphragm(),
                ShutterSpeed = exifReader.GetShutterSpeed(),
                ISO = exifReader.GetISO(),
                Flash = exifReader.GetFlash(),
                UserId = this.GetUserId()
            };

            await _photoService.UploadAsync(this.GetUserId(), photo);
            _fileService.Save(formFile.OpenReadStream(), originalPath);
            ImageProcessor.ResizeAndSaveImage(image, new Size(450, 300), thumbnailPath, 50);
            ImageProcessor.ResizeAndSaveImage(image, new Size(450, 300), midsizePath, 75);
            return NoContent();
        }

        /// <summary>
        /// Get photo by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/photos/{id}")]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Photo>))]
        public IActionResult GetPhoto(Guid id)
        {
            var photo = HttpContext.Items["entity"] as Photo;
            var photoDto = _mapper.Map<PhotoDto>(photo);
            return Ok(photoDto);
        }

        /// <summary>
        /// Edit photo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="photoDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/photos/{id}")]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Photo>))]
        public async Task<IActionResult> UpdatePhoto(Guid id, [FromBody]PhotoDto photoDto)
        {
            var photo = _mapper.Map<Photo>(photoDto);
            await _photoService.EditPhotoAsync(this.GetUserId(), photo);
            return NoContent();
        }

        /// <summary>
        /// Delete photo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/photos/{id}")]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Photo>))]
        public async Task<IActionResult> DeletePhoto(Guid id)
        {
            var thumbnailPath = _config["ThumbnailImgPath"];
            var midsizePath = _config["MidSizeImgPath"];
            var originalPath = _config["OriginalImgPath"];
            var photo = HttpContext.Items["entity"] as Photo;
            await _photoService.DeleteAsync(photo);

            _fileService.Delete(thumbnailPath + photo.FileName);
            _fileService.Delete(midsizePath + photo.FileName);
            _fileService.Delete(originalPath + photo.FileName);

            return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/photos/search")]
        public async Task<IActionResult> SearchPhotos([FromQuery]string param)
        {
            var photos = await _photoService.SearchPhotosAsync(this.GetUserId(), param);
            var photosDto = _mapper.Map<IReadOnlyList<PhotoDto>>(photos);
            return Ok(photosDto);
        }
    }
}
