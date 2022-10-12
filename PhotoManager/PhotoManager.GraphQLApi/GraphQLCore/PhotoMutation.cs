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
    public class PhotoMutation
    {
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Guid UserId =>
            Guid.Parse(_httpContextAccessor.HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public PhotoMutation(IPhotoService photoService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _photoService = photoService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Photo> EditPhoto(PhotoEditModel photoModel, IResolverContext context)
        {
            if (photoModel == null)
            {
                context.ReportError($"{nameof(PhotoModel)} is null or empty");
            }
            var photo = _mapper.Map<Photo>(photoModel);
            return await _photoService.EditPhotoAsync(UserId, photo);
        }

        public async Task<Photo> DeletePhoto(Guid photoId, IResolverContext context)
        {
            var photo = await _photoService.GetPhotoByIdAsync(photoId);
            
            if (photo == null)
            {
                context.ReportError($"Could not resolve a photo for the photoId: {photoId}.");
            }
            await _photoService.DeleteAsync(photo);
            return photo;
        }
    }
}
