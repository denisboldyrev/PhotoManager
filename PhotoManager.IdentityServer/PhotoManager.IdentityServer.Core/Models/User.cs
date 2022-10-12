using Microsoft.AspNetCore.Identity;
using System;

namespace PhotoManager.IdentityServer.Core.Models
{
    public class User : IdentityUser<Guid>
    {
        public static object Claims { get; set; }
        public int Year { get; set; }
    }
    public class UserRole : IdentityRole<Guid> {
        public UserRole(string name) : base(name)
        { 
        }
    }
    public partial class UserClaim : IdentityUserClaim<Guid>
    {
    }
    public partial class UserLogin : IdentityUserLogin<Guid>
    {
    }
    public partial class RoleClaim : IdentityRoleClaim<Guid>
    {
    }
    public partial class UserToken : IdentityUserToken<Guid>
    {
    }
}
