using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace PhotoManager.WebApp.Extensions
{
    public static class ControllerExtension
    {
        public static Guid GetUserId(this Controller controller) =>
            Guid.Parse(controller.HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
        
        public static string GetUserRole(this Controller controller) =>
            controller.HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.Role).Value;
    }
}
