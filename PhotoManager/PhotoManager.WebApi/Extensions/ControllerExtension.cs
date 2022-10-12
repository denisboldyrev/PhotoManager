using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace PhotoManager.WebApi.Extensions
{
    public static class ControllerBaseExtension
    {
        public static Guid GetUserId(this ControllerBase controller) =>
            Guid.Parse(controller.HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
        //public static Guid GetUserId(this ControllerBase controller) =>
        //    Guid.Parse("A2DC09DC-448D-424F-9180-5DBB96D873D9");
        public static string GetUserRole(this Controller controller) => 
            controller.HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.Role).Value;
    }
}
                