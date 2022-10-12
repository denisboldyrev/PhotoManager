using AutoMapper;
using PhotoManager.Core.Models;
using PhotoManager.WebApi.Models;

namespace PhotoManager.WebApi.Mapper
{
    public class PhotoMapperConfiguration : Profile
    {
        public PhotoMapperConfiguration()
        {
            CreateMap<Photo, PhotoDto>();
            CreateMap<PhotoDto, Photo>();
        }
    }
}
