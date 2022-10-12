using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoManager.WebApi.Filters
{
    public class ValidateImgExtAndSizeAttribute : IAsyncActionFilter
    {
        private readonly long _size = 512000;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {           
            var formFile = context.HttpContext.Request.Form.Files.FirstOrDefault();
          
            context.ModelState.Clear();

            if (formFile == null)
                context.Result = new NotFoundResult();

            var ext = Path.GetExtension(formFile.FileName).ToLowerInvariant();

            if (formFile.Length > _size)
            {
                context.ModelState.AddModelError("File Size", "Invalid File Size");
            }

            if (!(ext == ".jpeg" || ext == ".jpg"))
            {
                context.ModelState.AddModelError("Extension", "Invalid File Extension");
            }
                
            var fileName = Path.GetFileNameWithoutExtension(formFile.FileName);
            if (fileName.Length > 30)
            {
                context.ModelState.AddModelError("File Name", "File name is too long");
            }
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
                return;
            }
            context.HttpContext.Items.Add("formFile", formFile);
            await next();
        }
    }
}
