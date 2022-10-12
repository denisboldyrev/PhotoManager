using Microsoft.EntityFrameworkCore;
using PhotoManager.Core.Interfaces.Repository;
using PhotoManager.Core.Models;
using PhotoManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PhotoManager.Infrastructure.Repository
{
    public class AlbumRepository : Repository<Album>, IAlbumRepository
    {
        private readonly ApplicationContext _context;
        public AlbumRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Album> FindByNameAsync(string name)
        {
            var album = await _context.Albums.Where(a => a.Title == name).Include(p => p.Photos).ThenInclude(p => p.PhotoRatings).FirstOrDefaultAsync();
            return album;
        }
        public override async Task<Album> GetByIdAsync(Guid id)
        {
            return await _context.Albums.Include(p => p.Photos).SingleOrDefaultAsync(a => a.Id == id);
        }
        public override async Task<IReadOnlyList<Album>> GetAllAsync()
        {
            var albums = await _context.Albums.Include(p => p.Photos).ToListAsync();
            return albums;
        }
        public override async Task<IReadOnlyList<Album>> GetAllAsync(Expression<Func<Album, bool>> predicate)
        {
            var albums = await _context.Albums.Where(predicate).Include(p => p.Photos).ToListAsync();
            return albums;
        }
    }
}