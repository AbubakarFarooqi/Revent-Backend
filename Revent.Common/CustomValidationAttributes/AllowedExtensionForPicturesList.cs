using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.Common.CustomValidationAttributes
{
    public class AllowedExtensionForPicturesList : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionForPicturesList(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var files = value as List<IFormFile>;

            if (files != null)
            {
                foreach (var file in files)
                {
                    var extension = Path.GetExtension(file.FileName).ToLower();

                    if (!_extensions.Contains(extension))
                    {
                        return new ValidationResult($"Only the following file types are allowed: {string.Join(", ", _extensions)}");
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
