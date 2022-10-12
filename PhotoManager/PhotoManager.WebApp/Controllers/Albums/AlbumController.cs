using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PhotoManager.Core.Models;
using PhotoManager.WebApp.Models;
using System;
using PhotoManager.Core.Interfaces.Services;
using System.Threading;
using AutoMapper;
using PhotoManager.WebApp.Extensions;
using System.Collections.Generic;
using PhotoManager.WebApp.Filters;

namespace PhotoManager.WebApp.Controllers.Albums
{
    [Authorize]
    public class AlbumController : Controller
    {
        private readonly IAlbumService _albumService;
        private readonly IMapper _mapper;

        public AlbumController(IAlbumService albumService, IMapper mapper)
        {
            _albumService = albumService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var role = this.GetUserRole();
            var albums = await _albumService.GetAllAlbumsOfUserAsync(this.GetUserId());
            var albumsVM = _mapper.Map<AlbumVM[]>(albums);
            ViewBag.SubscriptionType = role;
            return View(albumsVM);
        }

        [HttpGet]
        [ServiceFilter(typeof(ValidateAlbumExistsAttribute))]
        public async Task<IActionResult> ManagePartial(Guid id)
        {
            var photos = await _albumService.GetAllPhotosFromAlbumAsync(id);
            var photosVM = _mapper.Map<PhotoVM[]>(photos);

            return PartialView("ManagePartial", photosVM);
        }

        [HttpGet]
        public async Task<IActionResult> AllAlbumsPartial()
        {
            var role = this.GetUserRole();
            var albums = await _albumService.GetAllAlbumsOfUserAsync(this.GetUserId());
            var albumsVM = _mapper.Map<AlbumVM[]>(albums);
            ViewBag.SubscriptionType = role;
            return PartialView("AllAlbumsPartial", albumsVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotosToAlbums([FromBody] AddPhotosToAlbumsVM data)
        {
            await _albumService.AddPhotosToAlbumsAsync(this.GetUserId(), data.Albums, data.Photos);
            return RedirectToAction("AllPhotosPartial","Photo");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePhotosFromAlbum([FromBody] DeletePhotosFromAlbumVM data)
        {
            await _albumService.DeletePhotosFromAlbumAsync(this.GetUserId(), data.AlbumId, data.PhotosId);
            return Json(new { success = true, message = "Photos have been deleted" });
        }

        [HttpGet]
        [Route("/Album/AllAlbumsModal")]
        public async Task<IActionResult> AllAlbumsModal()
        {
            var albums = await _albumService.GetAllAlbumsOfUserAsync(this.GetUserId());
            var albumsVM = _mapper.Map<IReadOnlyList<AlbumVM>>(albums);

            return PartialView("AllAlbumsModal", albumsVM);
        }
        /* *********************************************************************** */
        /* Action to get public link to album by name */
        /* *********************************************************************** */
        [HttpGet]
        [AllowAnonymous]
        [ServiceFilter(typeof(ValidateAlbumExistsAttribute))]
        [Route("/Albums/{title}")]
        public async Task<IActionResult> GetPublicAlbumAsync(string title)
        {
            var album = await _albumService.GetAlbumByTitleAsync(title);
            var photosVM = _mapper.Map<PhotoVM[]>(album.Photos);

            foreach (var item in photosVM)
            {
                item.IsLiked = _albumService.FindRating(this.GetUserId(), item.Id);
            }
            return View("GetAllPhotos", photosVM);
        }

        [HttpGet]
        [Route("/Album/Create")]
        public IActionResult Create()
        {          
            return View();
        }

        [AcceptVerbs("POST", "GET")]
        public async Task<IActionResult> CheckTitle(string title, string originalTitle)
        {
            if (title == originalTitle)
                return Json(true);
            var album = await _albumService.GetAlbumByTitleAsync(title);
            if(album == null)
                return Json(true);
            return Json($"Album already exists");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateUsersPermitionsAttribute<Album>))]
        public async Task<IActionResult> Create(AlbumVM albumVM)
        {
            var album = _mapper.Map<Album>(albumVM);
            album.UserId = this.GetUserId();

            await _albumService.CreateAlbumAsync(album);
            return RedirectToAction("Index", "Album");
        }

        [HttpGet]
        [ServiceFilter(typeof(ValidateAlbumExistsAttribute))]
        [Route("/Album/Edit/{id}")]
        public IActionResult Edit(Guid id)
        {
            var albumToEdit = HttpContext.Items["album"] as Album;
            var albumVM = _mapper.Map<AlbumVM>(albumToEdit);
            return View(albumVM);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidateAlbumExistsAttribute))]
        public async Task<IActionResult> Edit(Guid id, AlbumVM albumVM)
        {
            var album = HttpContext.Items["album"] as Album;
            var albumToUpdate = _mapper.Map<Album>(albumVM);

            if (albumToUpdate.Title == album.Title)
            {
                ModelState.ClearValidationState("Title");
                ModelState.MarkFieldValid("Title");
            }
            if (!ModelState.IsValid)
                return View(albumVM);

            await _albumService.EditAlbumAsync(albumToUpdate);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidateUserAttribute))]
        public async Task<IActionResult> Delete([FromBody]Guid id)
        {
            Thread.Sleep(1000);
            var album = HttpContext.Items["album"] as Album;
            await _albumService.DeleteAlbumAsync(album);
            return Json(new { success = true, message = "Album deleted successfully" });
        }
    }
}