using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoManager.Core.Models;
using System;
using PhotoManager.Core.Interfaces.Services;
using System.Threading;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Drawing;
using PhotoManager.BusinessLogic;
using PhotoManager.WebApp.Models;
using PhotoManager.WebApp.Extensions;
using AutoMapper;
using PhotoManager.WebApp.Filters;

namespace PhotoManager.WebApp.Controllers.Photos
{
    [Authorize]
    public class PhotoController : Controller
    {
        private readonly IPhotoService _photoService;
        private readonly IConfiguration _config;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public PhotoController(IPhotoService photoService, IConfiguration config, IFileService fileService, IMapper mapper)
        {
            _photoService = photoService;
            _fileService = fileService;
            _config = config;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //var role = this.GetUserRole();
            //ViewBag.CurrentUser = role;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AllPhotosPartial()
        {
            var photos = await _photoService.GetAllPhotosOfCurrentUserAsync(this.GetUserId());
            var photosVM = _mapper.Map<IReadOnlyList<PhotoVM>>(photos);
            return PartialView(photosVM);
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return PartialView("Upload");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(ValidateUsersPermitionsAttribute<Photo>))]
        public async Task<IActionResult> Upload(PhotoUploadVM formFile)
        {
            if (ModelState.IsValid)
            {
                var uniqueFileName = ImageProcessor.GetUniqueFileName(formFile.PhotoFile.FileName);

                var originalPath = _config["OriginalImgPath"] + uniqueFileName;
                var thumbnailPath = _config["ThumbnailImgPath"] + uniqueFileName;
                var midsizePath = _config["MidSizeImgPath"] + uniqueFileName;
                var image = Image.FromStream(formFile.PhotoFile.OpenReadStream());
                    var exifReader = new ExifReader(image);
                    var photoToAdd = new Photo
                    {
                        FileName = uniqueFileName,
                        Size = formFile.PhotoFile.Length,
                        Title = exifReader.GetTitle(),
                        DateOfTaking = exifReader.GetDate(),
                        CameraModel = exifReader.GetCameraModel(),
                        LensfocalLength = exifReader.GetFocalLength(),
                        Diaphragm = exifReader.GetDiaphragm(),
                        ShutterSpeed = exifReader.GetShutterSpeed(),
                        ISO = exifReader.GetISO(),
                        Flash = exifReader.GetFlash(),
                        UserId = this.GetUserId()
                    };

                    await _photoService.UploadAsync(this.GetUserId(), photoToAdd);

                    _fileService.Save(formFile.PhotoFile.OpenReadStream(), originalPath);

                    ImageProcessor.ResizeAndSaveImage(image, new Size(450, 300), thumbnailPath, 50);
                    ImageProcessor.ResizeAndSaveImage(image, new Size(450, 300), midsizePath, 75);

                    return Json(new { success = true, message = "Photo added successfully" });
            }
            else
            {
                var errorMessage = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return Json(new { success = false, message = errorMessage });
            }
        }

        [HttpGet]
        [Route("/Photo/Edit/{id}")]
        [ServiceFilter(typeof(ValidatePhotoExistsAttribute))]
        public IActionResult Edit(Guid id)
        {
            var photo = HttpContext.Items["photo"] as Photo;
            var photoVM = _mapper.Map<PhotoVM>(photo);
            return View(photoVM);
        }

        [HttpPost, ActionName("Edit")]
        [ServiceFilter(typeof(ValidatePhotoExistsAttribute))]
        public async Task<IActionResult> EditPost(Guid id, PhotoVM photoVM)
        {
            var photo = _mapper.Map<Photo>(photoVM);
            await _photoService.EditPhotoAsync(this.GetUserId(), photo);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] List<Guid> data)
        {
            var thumbnailPath = _config["ThumbnailImgPath"];
            var midsizePath = _config["MidSizeImgPath"];
            var originalPath = _config["OriginalImgPath"];


            var photos = await _photoService.GetAllPhotosOfCurrentUserAsync(this.GetUserId());
            var photosToDelete = photos.Where(p => data.Contains(p.Id)).Select(p=>p.FileName).ToList();
            Thread.Sleep(1000);

            await _photoService.DeleteRangeAsync(this.GetUserId(), data);

            foreach (var photo in photosToDelete)
            {
                _fileService.Delete(thumbnailPath + photo);
                _fileService.Delete(midsizePath + photo);
                _fileService.Delete(originalPath + photo);
            }
            return Json(new { success = true, message = "Photo has been deleted" });
        }

        [HttpGet]
        [Route("Photo/Detail/{id}")]
        [ServiceFilter(typeof(ValidatePhotoExistsAttribute))]
        public IActionResult Detail(Guid id)
        {
            var photo = HttpContext.Items["photo"] as Photo;
            var photoVM = _mapper.Map<PhotoVM>(photo);
            return View(photoVM);
        }

        [HttpGet]
        [Route("Photo/Search/")]
        public async Task<IActionResult> Search(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var photos = await _photoService.SearchPhotosAsync(this.GetUserId(), searchString);
            var photosVM = _mapper.Map<IReadOnlyList<PhotoVM>>(photos);
            return PartialView("AllPhotosPartial", photosVM);
            }

        [HttpPost]
        public async Task<IActionResult> Like([FromBody] Guid id)
        {
            var photo = await _photoService.GetPhotoByIdAsync(id);
            await _photoService.LikeAsync(this.GetUserId(), id);
            return Json(new { success = true, message = "" });        
        }
    }
}

