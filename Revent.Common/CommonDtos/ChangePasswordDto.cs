using System.ComponentModel.DataAnnotations;

namespace Revent.Common.CommonDtos
{
    public class ChangePasswordDto
    {

        [Required]
        public string Email { get; set; }

        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
