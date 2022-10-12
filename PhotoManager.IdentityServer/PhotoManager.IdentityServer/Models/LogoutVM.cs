using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoManager.IdentityServer.Models
{
    public class LogoutVM : LogoutInputModel
    {
        public bool ShowLogoutPrompt { get; set; } = true;
    }
}
