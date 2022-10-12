using Microsoft.AspNetCore.Identity;
using System;

namespace PhotoManager.Core
{
    public class User : IdentityUser<Guid>
    {
        public int Year { get; set; }
    }
    public class Role : IdentityRole<Guid> { 
    
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
