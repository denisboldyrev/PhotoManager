using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PhotoManager.Core.Interfaces.Repository;
using PhotoManager.Core.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PhotoManager.WebApi.Filters
{
    public class ValidateEntityExistsAttribute<T> : IAsyncActionFilter where T : BaseEntity, new()
    {
        private readonly IRepository<T> _repository;
        public ValidateEntityExistsAttribute(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userId = Guid.Parse(context.HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
#if TEST
            var userId = Guid.Parse("A2DC09DC-448D-424F-9180-5DBB96D873D9");
#endif
            var actionParams = context.ActionArguments;
            BaseEntity entity;
            if (context.ActionArguments.ContainsKey("id"))
            {
                var id = (Guid)context.ActionArguments["id"];
                entity = await _repository.GetByIdAsync(id);
            }
            else
            {
                context.Result = new BadRequestResult();
                return;
            }

            if(entity == null)
            {
                context.Result = new NotFoundResult();
                return;
            }
                
            if(entity.UserId != userId)
            {
                context.Result = new ForbidResult();
                return;
            }

            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
                return;
            }
            context.HttpContext.Items.Add("entity", entity);
            var result = await next();
        }
    }
}
