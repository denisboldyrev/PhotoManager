using AutoMapper;
using PhotoManager.Core.Models;
using PhotoManager.GraphQLApi.Models;

namespace PhotoManager.GraphQLApi.Mapper
{
    public class PhotoMapperConfiguration : Profile
    {
        public PhotoMapperConfiguration()
        {
            CreateMap<Photo, PhotoModel>();
            CreateMap<PhotoModel, Photo>();
            CreateMap<Photo, PhotoEditModel>();
            CreateMap<PhotoEditModel, Photo>();
        }
    }
}
