using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.AspNetCore.Authorization;
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
    public class PhotoQuery
    {
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Guid UserId =>
           Guid.Parse(_httpContextAccessor.HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public PhotoQuery(IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Photo> Photo(Guid photoId, IResolverContext context)
        {
            var photo = await _photoService.GetPhotoByIdAsync(photoId);
            
            if (photo == null)
            {
                context.ReportError($"Could not resolve a photo for the photoId: {photoId}.");
            }
            return photo;
        }

        public async Task<IReadOnlyList<Photo>> Photos()
        {
            return await _photoService.GetAllPhotosOfCurrentUserAsync(UserId);
        }
    }
}
