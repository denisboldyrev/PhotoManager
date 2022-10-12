using PhotoManager.Core.Models;
using PhotoManager.Data;


namespace PhotoManager.Infrastructure.Repository
{
    public class AppUserRepository : Repository<AppUser>
    {
        public AppUserRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
