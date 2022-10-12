using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace PhotoManager.WebApp.Attributes
{
    public class PhotoFileValidatorAttribute : ValidationAttribute
    {
        public long Size { get; }
        public PhotoFileValidatorAttribute(long size)
        {
            Size = size;
        }
        protected override ValidationResult IsValid(
               object value, ValidationContext validationContext)
        {
            var photo = (IFormFile)value;
            if (photo == null)
            {
                return new ValidationResult("Please, select photo");
            }
            var ext = Path.GetExtension(photo.FileName).ToLowerInvariant();

            if (photo.Length > Size)
                return new ValidationResult("Invalid File Size");
            if (!(ext.Contains("jpeg") || ext.Contains("jpg")))
                return new ValidationResult("Invalid File Extension");
            var fileName = Path.GetFileNameWithoutExtension(photo.FileName);
            if (fileName.Length > 30)
                return new ValidationResult("File name is too long");

            return ValidationResult.Success;
        }
    }
}