using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PhotoManager.Core.Interfaces.Services;
using PhotoManager.Core.Models;
using PhotoManager.WebApi.Extensions;
using PhotoManager.WebApi.Filters;
using PhotoManager.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoManager.WebApi.Controllers
{
 //   [Authorize]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private readonly IAlbumService _albumService;
        private readonly IMapper _mapper;
      
        public AlbumsController(IAlbumService albumService, IMapper mapper)
        {
            _albumService = albumService;
            _mapper = mapper;
        }
        /// <summary>
        /// Get all albums of user
        /// </summary>
        /// <returns>All albums of current user</returns>
        /// 
        [HttpGet]
        [ProducesDefaultResponseType]
        [Route("api/albums")]
        public async Task<IActionResult> GetAlbums()
        {
            var albums = await _albumService.GetAllAlbumsOfUserAsync(this.GetUserId());
            var albumsDto = _mapper.Map<IReadOnlyList<AlbumDto>>(albums);
            return Ok(albumsDto);
        }
        /// <summary>
        /// Create new album
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/albums/
        ///     {
        ///        "title": "Test album",
        ///        "description": "Test Description"
        ///     }
        ///
        /// </remarks>
        /// <param name="albumDto"></param>
        /// <returns>A newly created Album</returns>
        /// <response code="201">Returns the newly created Album</response>
        /// <response code="400">If the album already exists</response>      
        [HttpPost]
        [Route("api/albums/")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        //[ServiceFilter(typeof(ValidateUsersPermitionsAttribute<Album>))]
        public async Task<IActionResult> CreateAlbum([FromBody]AlbumDto albumDto)
        {
            var album = await _albumService.GetAlbumByTitleAsync(albumDto.Title);
            if (album != null)
                return BadRequest($"Album with Title: \"{albumDto.Title}\" already exists");
            album = _mapper.Map<AlbumDto, Album>(albumDto);
            album.UserId = this.GetUserId();
            await _albumService.CreateAlbumAsync(album);
            var createdAlbum = _mapper.Map<AlbumDto>(album);
            return CreatedAtRoute("GetAlbum", new { id = createdAlbum.Id }, createdAlbum);
        }

        /// <summary>
        /// Get Album by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET api/albums/9e462cc9-1637-43f6-6e99-08d95bf76e2e
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Returns current album</response>
        /// <response code="401">If user not authorized</response>
        /// <response code="403">If user is not an owner</response> 
        /// <response code="404">If the album is null</response> 
        [HttpGet]
        [Route("api/albums/{id}", Name = "GetAlbum")]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Album>))]
        public IActionResult GetAlbum(Guid id)
        {
            var album = HttpContext.Items["entity"] as Album;
            var albumDto = _mapper.Map<AlbumDto>(album);

            return Ok(albumDto);
        }

        /// <summary>
        /// Delete album
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/albums/{id}")]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Album>))]
        public async Task<IActionResult> DeleteAlbum(Guid id)
        {
            var album = HttpContext.Items["entity"] as Album;
            await _albumService.DeleteAlbumAsync(album);

            return NoContent();
        }

        /// <summary>
        /// Edit Album
        /// </summary>
        ///  /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/albums/{id}
        ///     {
        ///        "title": "Test album",
        ///        "description": "Test Description"
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="albumDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/albums/{id}")]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Album>))]
        public async Task<IActionResult> Put(Guid id, AlbumDto albumDto)
        {
            var album = _mapper.Map<Album>(albumDto);
            await _albumService.EditAlbumAsync(album);
            return NoContent();
        }

        /// <summary>
        /// Get all photos from album
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("api/albums/{id}/photos")]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Album>))]
        public async Task<IActionResult> GetAllPhotosFromAlbum(Guid id)
        {
            var photos = await _albumService.GetAllPhotosFromAlbumAsync(id);
            var photosDto = _mapper.Map<IReadOnlyList<PhotoDto>>(photos);
            return Ok(photosDto);
        }

        /// <summary>
        /// Add photo to album
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="photoId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/albums/{albumId}/photos")]
        public async Task<IActionResult> AddPhotoToAlbum(Guid albumId, [FromBody]Guid photoId)
        {
            var album = await _albumService.GetAlbumAsync(this.GetUserId(), albumId);
            if (album == null)
                return NotFound();

            if (album.UserId != this.GetUserId())
                return Unauthorized();

            await _albumService.AddPhotoToAlbumAsync(albumId, photoId);
            return NoContent();
        }

        /// <summary>
        /// Delete photo from album
        /// </summary>
        /// <param name="albumId"></param>
        /// <param name="photoId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/albums/{albumId}/photos/{photoId}")]
        public async Task<IActionResult>DeletePhotoFromAlbum(Guid albumId, Guid photoId)
        {
            var album = await _albumService.GetAlbumAsync(this.GetUserId(),albumId);
            if (album == null)
                return NotFound();
            
            if (album.UserId != this.GetUserId())
                return Unauthorized();

            var photo = album.Photos.SingleOrDefault(p => p.Id == photoId);
            if (photo == null)
                return NotFound();

            await _albumService.DeletePhotoFromAlbumAsync(albumId, photoId);

            return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/albums/validateTitleExists")]
        public async Task<IActionResult> CheckTitle(string title)
        {
                var album = await _albumService.GetAlbumByTitleAsync(title);
                if (album != null)
                    return Ok(true);
                return Ok(false);
        }
    }
}
    