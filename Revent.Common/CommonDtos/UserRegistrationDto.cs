using Microsoft.AspNetCore.Http;
using Revent.Common.CustomValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Revent.Common.CommonDtos
{
    public class UserRegistrationDto
    {
        [Required(ErrorMessage = "Email is required")]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(255)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Firstname is required")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        [StringLength(50)]
        public string LastName { get; set; }

        [AllowedExtensionForPictures(new string[] { ".jpg", ".jpeg", ".png" })]
        public IFormFile? ProfileImage { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
