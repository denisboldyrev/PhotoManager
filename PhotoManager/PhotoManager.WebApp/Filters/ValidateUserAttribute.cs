using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PhotoManager.Core.Interfaces.Repository;
using PhotoManager.Core.Models;
using PhotoManager.WebApp.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PhotoManager.WebApp.Filters
{
    public class ValidateUserAttribute : IAsyncActionFilter
    {
        private readonly IUnitOfWork _unitOfWork;
        public ValidateUserAttribute(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Album album;
            var userId = Guid.Parse(context.HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (context.ActionArguments.ContainsKey("id"))
            {
                var id = (Guid)context.ActionArguments["id"];
                album = await _unitOfWork.Albums.GetByIdAsync(id);
            }
            else
            {
                var error = new ErrorVM { ErrorMessage = $"{nameof(album)} not found", StatusCode = context.HttpContext.Response.StatusCode = 404 };
                context.Result = new RedirectToActionResult("Error", "Home", error);
                return;
            }

            if (album != null && album.UserId == userId)
            {
                context.HttpContext.Items.Add("album", album);
            }
            else
            {
                context.Result = new NotFoundResult();
            }
            var result = await next();
        }
    }
}
