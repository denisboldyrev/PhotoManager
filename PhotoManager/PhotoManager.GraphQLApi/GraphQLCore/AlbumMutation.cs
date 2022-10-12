using AutoMapper;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using PhotoManager.Core.Interfaces.Services;
using PhotoManager.Core.Models;
using PhotoManager.GraphQLApi.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PhotoManager.GraphQLApi.GraphQLCore
{
    [ExtendObjectType(Name = "Mutation")]
    [Authorize]
    public class AlbumMutation
    {
        private readonly IAlbumService _albumService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Guid UserId =>
            Guid.Parse(_httpContextAccessor.HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public AlbumMutation(IAlbumService albumService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _albumService = albumService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Album> CreateAlbum(AlbumModel albumModel, IResolverContext context)
        {
            if (albumModel == null)
            {
                context.ReportError($"Input model is null or empty");
            }
            var album = _mapper.Map<Album>(albumModel);
            return await _albumService.CreateAlbumAsync(album);
        }

        public async Task<Album> EditAlbum(AlbumEditModel albumEditModel, IResolverContext context)
        {
            if (albumEditModel == null)
            {
                context.ReportError($"Input model is null or empty");
            }
            var album = _mapper.Map<Album>(albumEditModel);
            return await _albumService.EditAlbumAsync(album);
        }

        public async Task<Album> DeleteAlbum(Guid albumId, IResolverContext context)
        {
            var album = await _albumService.GetAlbumAsync(UserId, albumId);

            if (album == null)
            {
                context.ReportError($"Could not resolve an album for the albumId: {albumId}.");
            }
            await _albumService.DeleteAlbumAsync(album);
            return album;
        }
    }
}
