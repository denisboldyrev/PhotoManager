using Microsoft.AspNetCore.Http;
using PhotoManager.WebApp.Attributes;
using System.ComponentModel.DataAnnotations;

namespace PhotoManager.WebApp.Models
{
    public class PhotoUploadVM
    {
        [PhotoFileValidator(512000)]
        public IFormFile PhotoFile { get; set; }
    }
}
