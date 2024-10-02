using Revent.Common.CommonDtos;
using Revent.Common.CommonModels;
namespace Revent.Services.IServices
{
    public interface IUserService
    {
        Task<ApplicationUser?> AddUserAsync(UserRegistrationDto user, bool isAdmin = false);
        Task<List<string>?> GetUserRoles(ApplicationUser user);
        Task<ApplicationUser> AddUserWithoutPasswordAsync(UserRegistrationDto user);
        Task<ApplicationUser?> FindUserAsync(string email);
        Task<UserProfileDto> GetUserProfileAsync(string email);
        //Task UpdateUserProfileAsync(UserProfileDto userProfile);
        Task ChangeUserPasswordAsync(ApplicationUser user, ChangePasswordDto model);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    }
}
