using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PhotoManager.Core.Interfaces.Repository;
using PhotoManager.Core.Models;
using PhotoManager.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PhotoManager.WebApp.Filters
{
    public class ValidatePhotoExistsAttribute : IAsyncActionFilter
    {
        private readonly IUnitOfWork _unitOfWork;

        public ValidatePhotoExistsAttribute(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userId = Guid.Parse(context.HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            Photo photo;
            if (context.ActionArguments.ContainsKey("id"))
            {
                var id = (Guid)context.ActionArguments["id"];
                photo = await _unitOfWork.Photos.GetByIdAsync(id);
            }
            else
            {
                var error = new ErrorVM { ErrorMessage = "Photo not found", StatusCode = context.HttpContext.Response.StatusCode = 404 };
                context.Result = new RedirectToActionResult("Error", "Home", error);
                return;
            }

            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
                return;
            }

            if (photo != null && photo.UserId == userId)
            {
                context.HttpContext.Items.Add("photo", photo);
            }
            else
            {
                context.Result = new NotFoundResult();
            }
            var result = await next();
        }
    }
}
