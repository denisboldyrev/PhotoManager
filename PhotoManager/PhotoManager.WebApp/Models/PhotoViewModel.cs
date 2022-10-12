using Microsoft.AspNetCore.Http;
using PhotoManager.WebApp.Attributes;

namespace PhotoManager.WebApp.Models.ViewModels
{
  
    public class PhotoViewModel
    {
        [PhotoFileValidator(512000)]
        public IFormFile PhotoFile { get; set; }
    }
}
