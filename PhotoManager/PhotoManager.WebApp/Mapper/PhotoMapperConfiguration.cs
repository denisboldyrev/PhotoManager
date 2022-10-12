using AutoMapper;
using PhotoManager.Core.Models;
using PhotoManager.WebApp.Models;

namespace PhotoManager.WebApp.Mapper
{
    public class PhotoMapperConfiguration : Profile
    {
        public PhotoMapperConfiguration()
        {
            CreateMap<Photo, PhotoVM>();
            CreateMap<PhotoVM, Photo>();
        }
    }
}
