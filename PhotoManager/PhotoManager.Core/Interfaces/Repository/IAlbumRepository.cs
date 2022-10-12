using PhotoManager.Core.Models;
using System;
using System.Threading.Tasks;

namespace PhotoManager.Core.Interfaces.Repository
{
    public interface IAlbumRepository : IRepository<Album>
    {
        Task<Album> FindByNameAsync(string name);
    }
}
