using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PhotoManager.WebApp.Models;
using System.Linq;

namespace PhotoManager.WebApp.Filters
{
    public class ValidationFilterAttribute : IActionFilter
    {

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var param = context.ActionArguments.SingleOrDefault(p => p.Value is AlbumVM);
            if (param.Value == null)
            {
                context.Result = new NotFoundObjectResult(context.ModelState);
                return;
            }
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
           
        }
      
    }
}
