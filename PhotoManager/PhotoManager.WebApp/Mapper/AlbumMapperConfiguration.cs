using AutoMapper;
using PhotoManager.Core.Models;
using PhotoManager.WebApp.Models;

namespace PhotoManager.WebApp.Mapper
{
    public class AlbumMapperConfiguration : Profile
    {
        public AlbumMapperConfiguration()
        {
            CreateMap<Album, AlbumVM>();
            CreateMap<AlbumVM, Album>();           
        }
    }
}
