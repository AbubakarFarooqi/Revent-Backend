using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.Common.CustomValidationAttributes
{
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentValue = (DateOnly)value;
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
                throw new ArgumentException("Property with this name not found");

            var comparisonValue = (DateOnly)property.GetValue(validationContext.ObjectInstance);

            if (currentValue <= comparisonValue)
            {
                return new ValidationResult(ErrorMessage ?? "End date must be greater than start date");
            }

            return ValidationResult.Success;
        }
    }
}
