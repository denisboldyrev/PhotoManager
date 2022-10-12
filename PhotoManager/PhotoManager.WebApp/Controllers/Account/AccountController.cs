using Microsoft.AspNetCore.Mvc;

namespace PhotoManager.WebApp.Controllers.Account
{
    public class AccountController : Controller
    {


        public AccountController()
        {
           
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Logout() { 
            return SignOut("Cookies", "oidc"); 
        }
    }
}
