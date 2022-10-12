using PhotoManager.Data;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PhotoManager.Core.Attributes
{
    public class ValidationUniqueTitleAttribute : ValidationAttribute
    {
        public string GetErrorMessage() => $"Album already exists";
        protected override ValidationResult IsValid(
               object value, ValidationContext validationContext)
        {
            var normalizedValue = value.ToString().ToLower().Trim();
            var _context = (ApplicationContext)validationContext.GetService(typeof(ApplicationContext));
            var albumDb = _context.Albums.FirstOrDefault(a => a.Title.ToLower() == normalizedValue);
            if (albumDb == null)
                return ValidationResult.Success;
            return new ValidationResult(GetErrorMessage());
        }
    }
}