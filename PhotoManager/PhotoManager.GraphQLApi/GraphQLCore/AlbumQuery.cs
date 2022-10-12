using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using PhotoManager.Core.Interfaces.Services;
using PhotoManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PhotoManager.GraphQLApi.GraphQLCore
{
    [ExtendObjectType(Name = "Query")]
    [Authorize]
    public class AlbumQuery
    {
        private readonly IAlbumService _albumService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Guid UserId =>
           Guid.Parse(_httpContextAccessor.HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public AlbumQuery(IAlbumService albumService, IHttpContextAccessor httpContextAccessor)
        {
            _albumService = albumService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IReadOnlyList<Album>> Albums()
        {
            return await _albumService.GetAllAlbumsOfUserAsync(UserId);
        }

        public async Task<Album> Album(Guid albumId, IResolverContext context)
        {
            var album = await _albumService.GetAlbumAsync(UserId, albumId);

            if (album == null)
            {
                context.ReportError($"Could not resolve an album for the albumId: {albumId}.");
            }

            return album;
        }

        public async Task<IReadOnlyList<Photo>> GetPhotosFromAlbum(Guid albumId)
        {
            return await _albumService.GetAllPhotosFromAlbumAsync(albumId);
        }
    }
}
