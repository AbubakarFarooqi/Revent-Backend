using AutoMapper;
using Microsoft.Extensions.Configuration;
using Revent.Common.CommonDtos;
using Revent.Common.CommonModels;
using Revent.DataAccess.Implementation.Migrations;
using Revent.DataAccess.Implementation.UnitOfWork;
using Revent.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Revent.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UserService(IMapper mapper, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<ApplicationUser?> AddUserAsync(UserRegistrationDto user, bool isAdmin = false)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var appUser = _mapper.Map<ApplicationUser>(user);
                appUser.CreatedAt = DateTime.UtcNow;
                appUser.UserName = user.Email;

                bool isUserAdded = await _unitOfWork.UserRepository.AddAsync(appUser, user.Password);

                if (!isUserAdded) return null;
                
                bool isUserAddedInRole;
                
                if (isAdmin)
                    isUserAddedInRole = await _unitOfWork.UserRepository.AddToRoleAsync(appUser, new List<string> { "admin"});
                else
                    isUserAddedInRole = await _unitOfWork.UserRepository.AddToRoleAsync(appUser, new List<string> { "appUser" });

                if (!isUserAddedInRole)
                {
                    await _unitOfWork.UserRepository.DeleteAsync(appUser);
                    return null;
                }
                await _unitOfWork.CommitTransactionAsync();

                return appUser;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<ApplicationUser> AddUserWithoutPasswordAsync(UserRegistrationDto user)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var appUser = _mapper.Map<ApplicationUser>(user);
                appUser.CreatedAt = DateTime.UtcNow;
                appUser.UserName = user.Email;

                bool isUserAdded = await _unitOfWork.UserRepository.AddWithoutPasswordAsync(appUser);
                
                if (!isUserAdded) return null;

                bool isUserAddedInRole = await _unitOfWork.UserRepository.AddToRoleAsync(appUser, new List<string> { "appUser1" });

                if(!isUserAddedInRole) 
                {
                    await _unitOfWork.UserRepository.DeleteAsync(appUser);
                    return null;
                }

                await _unitOfWork.CommitTransactionAsync();

                return appUser;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task ChangeUserPasswordAsync(ApplicationUser user,ChangePasswordDto model)
        {
            try
            {
                // this should be in controller layer
                //if (model.NewPassword != model.ConfirmPassword)
                //{
                //    return new ServiceResponse<bool>
                //    {
                //        Success = false,
                //        ErrorMessage = "New Paassword is not equal to confirm password"
                //    };
                //}
                //if (model.NewPassword == model.CurrentPassword)
                //{
                //    return new ServiceResponse<bool>
                //    {
                //        Success = false,
                //        ErrorMessage = "New password cannot be same as old password"
                //    };
                //}

                await _unitOfWork.UserRepository.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                await _unitOfWork.SaveChangesAsync();
                return;

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await _unitOfWork.UserRepository.CheckPasswordAsync(user, password);
        }

        public async Task<ApplicationUser?> FindUserAsync(string email)
        {
            return await _unitOfWork.UserRepository.FindAsync(email);
        }

        public async Task<UserProfileDto> GetUserProfileAsync(string email)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.FindAsync(email);
                
                if (user == null) return null;

                var userRoles = await _unitOfWork.UserRepository.GetRoleAsync(user);

                UserProfileDto userProfile = _mapper.Map<UserProfileDto>(user);
                userProfile.Roles = userRoles;

                return userProfile;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<string>?> GetUserRoles(ApplicationUser user)
        {
            return await _unitOfWork.UserRepository.GetRoleAsync(user);
        }

        /* public async Task UpdateUserProfileAsync(UserProfileDto userProfile)
         {
             try
             {
                 await _unitOfWork.BeginTransactionAsync();

                 var repoResponse = await _unitOfWork.UserRepository.GetAsync(userProfile.Email);
                 if (!repoResponse.Success)
                 {
                     await _unitOfWork.RollbackTransactionAsync();

                     return new ServiceResponse<bool>
                     {
                         Success = false,
                         ErrorMessage = repoResponse.ErrorMessage
                     };
                 }

                 User user = repoResponse.Data;
                 var userRoleResponse = await _unitOfWork.UserRepository.GetRoleAsync(repoResponse.Data);

                 if (!userRoleResponse.Success)
                 {
                     await _unitOfWork.RollbackTransactionAsync();

                     return new ServiceResponse<bool>
                     {
                         Success = false,
                         ErrorMessage = repoResponse.ErrorMessage
                     };
                 }

                 if (userRoleResponse.Data[0] != userProfile.Role)
                 {
                     List<string> roles = [userRoleResponse.Data[0]];

                     var isRoleRemoved = await _unitOfWork.UserRepository.RemoveFromRoleAsync(user, roles);
                     if (!isRoleRemoved.Success)
                     {
                         await _unitOfWork.RollbackTransactionAsync();

                         return new ServiceResponse<bool>
                         {
                             Success = false,
                             ErrorMessage = repoResponse.ErrorMessage
                         };
                     }

                     var isRoleUpdated = await _unitOfWork.UserRepository.AddToRoleAsync(user, userProfile.Role);
                     if (!isRoleUpdated.Success)
                     {
                         await _unitOfWork.RollbackTransactionAsync();

                         return new ServiceResponse<bool>
                         {
                             Success = false,
                             ErrorMessage = repoResponse.ErrorMessage
                         };
                     }
                 }


                 user.FirstName = userProfile.FirstName;
                 user.LastName = userProfile.LastName;

                 var isUpdated = await _unitOfWork.UserRepository.UpdateAsync(user);
                 if (!isUpdated.Success)
                 {
                     await _unitOfWork.RollbackTransactionAsync();

                     return new ServiceResponse<bool>
                     {
                         Success = false,
                         ErrorMessage = repoResponse.ErrorMessage
                     };
                 }

                 await _unitOfWork.CommitTransactionAsync();

                 return new ServiceResponse<bool>
                 {
                     Success = true,
                 };


             }
             catch (Exception ex)
             {
                 await _unitOfWork.RollbackTransactionAsync();

                 return new ServiceResponse<bool>
                 {
                     Success = false,
                     ErrorMessage = ex.Message
                 };
             }
         }*/
    }
}
