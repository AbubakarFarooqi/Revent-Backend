using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.Common.CommonDtos
{
    public class UserRegistrationDto
    {
        [Required(ErrorMessage = "Email is required")]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(255)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Firstname is required")]
        [StringLength(50)]
        public string FullName { get; set; }
        public string? ProfilePicture { get; set; }
    }
}
