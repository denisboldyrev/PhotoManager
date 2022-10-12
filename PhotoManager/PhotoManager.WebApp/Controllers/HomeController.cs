using PhotoManager.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Serilog;
using Microsoft.AspNetCore.Diagnostics;
using PhotoManager.Core.Interfaces.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using PhotoManager.WebApp.Extensions;
using System.Collections.Generic;

namespace PhotoManager.WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAlbumService _albumService;
        private readonly IMapper _mapper;

        public HomeController(IAlbumService albumService, IMapper mapper)
        {
            _albumService = albumService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var albums = await _albumService.GetAllAlbumsOfUserAsync(this.GetUserId());
            var albumsVM = _mapper.Map<IReadOnlyList<AlbumVM>>(albums);
            return View(albumsVM);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public IActionResult Error([FromQuery]ErrorVM error)
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature != null)
                Log.ForContext("RequestId", $"{requestId}").Error(exceptionHandlerPathFeature.Error, exceptionHandlerPathFeature.Error.Message);
            error.RequestId = requestId;
            Log.ForContext("RequestId", $"{requestId}").Error(error.ErrorMessage, error.StatusCode);
            return View(error);
        }
    }
}
