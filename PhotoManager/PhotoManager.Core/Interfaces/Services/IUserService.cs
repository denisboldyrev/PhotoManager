using PhotoManager.Core.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PhotoManager.Core.Interfaces.Services
{
    public interface IUserService
    {
        public Task AddUserAsync(Guid userId);
        public Task<AppUser> GetCurrentUserByIdAsync(ClaimsPrincipal user); 
    }
}
