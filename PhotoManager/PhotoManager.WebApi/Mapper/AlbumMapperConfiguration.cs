using AutoMapper;
using PhotoManager.Core.Models;
using PhotoManager.WebApi.Models;

namespace PhotoManager.WebApi.Mapper
{
    public class AlbumMapperConfiguration : Profile
    { 
        public AlbumMapperConfiguration()
        {
            CreateMap<Album, AlbumDto>();
            CreateMap<AlbumDto, Album>();
        }
    }
}
