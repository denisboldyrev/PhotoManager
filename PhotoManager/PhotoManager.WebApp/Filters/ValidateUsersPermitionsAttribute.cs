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
    public class ValidateUsersPermitionsAttribute<T> : IAsyncActionFilter where T : BaseEntity, new()
    {
        private readonly IRepository<T> _repository;
        public ValidateUsersPermitionsAttribute(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userId = Guid.Parse(context.HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var userRole = context.HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.Role).Value;
            
            var actionParams = context.ActionArguments;

            var allEntities = await _repository.GetAllAsync(e=>e.UserId == userId);
            var countEntities = allEntities.Count;

            T entity = new();
            if (entity is Album)
            {
                if (countEntities >= 5 && context.HttpContext.User.IsInRole("RegularUser"))
                {
                    context.ModelState.AddModelError("Albums", $"Warning! Regular user is allowed to create up to 5 albums. Current amount is {countEntities}. Please, update your subscription.");
                    context.Result = new BadRequestObjectResult(context.ModelState);
                    return;
                }
            }
            if (entity is Photo)
            {
                if (countEntities >= 60 && context.HttpContext.User.IsInRole("RegularUser"))
                {
                    context.ModelState.AddModelError("Photos", $"Warning! Regular user is allowed to upload up to 60 photos. Current amount is {countEntities}. Please, update your subscription.");
                    context.Result = new BadRequestObjectResult(context.ModelState);
                    return;
                }
            }
            var result = await next();
        }
    }
}
