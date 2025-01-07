using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Revent.Common.CustomValidationAttributes
{
    public class PhoneNumberValidator: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success; // Null or empty phone numbers are considered valid 
            }

            // Regex pattern to validate phone number with country code
            string phonePattern = @"^\+(\d{1,4})[\s\-\(\)]?(\d{1,15})$";

            // Validate phone number format
            var phoneNumber = value.ToString();
            if (Regex.IsMatch(phoneNumber, phonePattern))
            {
                return ValidationResult.Success; // The phone number is valid
            }
            else
            {
                return new ValidationResult("The phone number is not in a valid format. It should include a country code followed by the number.");
            }
        }
    }
}
