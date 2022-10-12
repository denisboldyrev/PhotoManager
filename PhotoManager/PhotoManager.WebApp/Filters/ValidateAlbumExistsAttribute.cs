using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PhotoManager.Core.Interfaces.Repository;
using PhotoManager.Core.Models;
using PhotoManager.WebApp.Models;
using System;
using System.Threading.Tasks;

namespace PhotoManager.WebApp.Filters
{
    public class ValidateAlbumExistsAttribute : IAsyncActionFilter
    {
        private readonly IUnitOfWork _unitOfWork;

        public ValidateAlbumExistsAttribute(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Album album;
            if (context.ActionArguments.ContainsKey("id"))
            {
                var id = (Guid)context.ActionArguments["id"];
                album = await _unitOfWork.Albums.GetByIdAsync(id);
            }
            else if (context.ActionArguments.ContainsKey("title"))
            {
                var title = (string)context.ActionArguments["title"];
                album = await _unitOfWork.Albums.FindByNameAsync(title);
            }
            else
            {
                
                var error = new ErrorVM { ErrorMessage = "Album not found", StatusCode = context.HttpContext.Response.StatusCode = 404 };
                context.Result = new RedirectToActionResult("Error", "Home", error);
                return;
            }

            if (album != null)
            {
                context.HttpContext.Items.Add("album", album);
            }
            else
            {
                context.Result = new NotFoundResult();
            }
            await next();
        }
    }
}
