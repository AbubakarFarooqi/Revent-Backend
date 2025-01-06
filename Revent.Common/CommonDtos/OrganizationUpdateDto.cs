using Revent.Common.CustomValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.Common.CommonDtos
{
    public class OrganizationUpdateDto
    {
        [Required]
        public int OrganizationId { get; set; }
        [Required]

        public string Address { get; set; }
        [Required]

        public int OrganizationType { get; set; }
    }
}
