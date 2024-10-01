using Microsoft.AspNetCore.Identity;

namespace Revent.Common.CommonModels
{
    public class ApplicationUser:IdentityUser
    {
        public string ProfilePicture { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
