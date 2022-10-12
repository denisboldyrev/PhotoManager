using AutoMapper;
using PhotoManager.Core.Models;
using PhotoManager.GraphQLApi.Models;

namespace PhotoManager.GraphQLApi.Mapper
{
    public class AlbumMapperConfiguration : Profile
    {
        public AlbumMapperConfiguration()
        {
            CreateMap<Album, AlbumModel>();
            CreateMap<AlbumModel, Album>();
            CreateMap<Album, AlbumEditModel>();
            CreateMap<AlbumEditModel, Album>();
        }
    }
}
