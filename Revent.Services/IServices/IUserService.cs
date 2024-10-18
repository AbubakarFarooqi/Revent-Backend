using Microsoft.AspNetCore.Identity;
using Revent.Common.CommonDtos;
using Revent.Common.CommonModels;
using Revent.DataAccess.Implementation.IRepositories;
using Revent.EFCore.DataModel.Models;
namespace Revent.Services.IServices
{
    public interface IUserService
    {
        Task<Users?> AddUserAsync(UserRegistrationDto user, bool isAdmin = false);
        Task<Users> AddUserWithoutPasswordAsync(UserRegistrationDto user);
        Task<List<string>?> GetUserRoles(Users user);
        Task<Users?> FindUserAsync(string email);
        Task<Users?> FindUserById(int Id);
        //Task<UserProfileDto> GetUserProfileAsync(string email);
        //Task UpdateUserProfileAsync(UserProfileDto userProfile);
        Task ChangeUserPasswordAsync(Users user, ChangePasswordDto model);
        Task<bool> CheckPasswordAsync(Users user, string password);
    }
}
