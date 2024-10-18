using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.Common.CustomValidationAttributes
{
    public class AllowedExtensionForPictures : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionForPictures(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_extensions.Contains(extension))
                {
                    return new ValidationResult("This file extension is not allowed.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
