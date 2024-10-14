using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.Common.Extensions
{
    public static class ModelStateExtensions
    {
        public static string GetValidationErrorsAsCsv(this ModelStateDictionary modelState)
        {
            var errors = modelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

            return string.Join(", ", errors);
        }
    }
}
