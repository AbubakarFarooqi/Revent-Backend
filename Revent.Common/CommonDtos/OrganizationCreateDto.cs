using Revent.Common.CustomValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.Common.CommonDtos
{
    public class OrganizationCreateDto
    {
        [Required]
        public int UserId { get; set; }
        public string? ContactNumber { get; set; }
        public string Address { get; set; }
        [Required]
        public int OrganizationType { get; set; }
    }
}
