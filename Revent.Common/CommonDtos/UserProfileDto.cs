using System.ComponentModel.DataAnnotations;

namespace Revent.Common.CommonDtos
{
    public class UserProfileDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string ProfileImage { get; set; }
        [Required]

        public string Email { get; set; }
        [Required]
        public List<string> Roles { get; set; }

    }
}
