using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PhotoManager.WebApi.Models
{
    public class PhotoUploadDto
    {
        [Required]
        public IFormFile PhotoFile { get; set; }
    }
}
