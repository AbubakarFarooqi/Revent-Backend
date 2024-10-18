using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Revent.Common.CommonDtos;
using Revent.Common.CommonModels;
using Revent.DataAccess.Implementation.UnitOfWork;
using Revent.EFCore.DataModel.Models;
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
        private readonly ICloudinaryService _cloudinaryService;

        public UserService(IMapper mapper, IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cloudinaryService = cloudinaryService;
        }
        public async Task<Users?> AddUserAsync(UserRegistrationDto user, bool isAdmin = false)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var appUser = _mapper.Map<Users>(user);

                //Adding asp user
                IdentityUser aspUser = new IdentityUser();
                
                aspUser.UserName = user.Email;
                aspUser.Email = user.Email;

                bool isAspUserAdded = await _unitOfWork.UserRepository.AddAspUserAsync(aspUser, user.Password);


                if (!isAspUserAdded) return null;
                
                bool isUserAddedInRole;
                
                if (isAdmin)
                    isUserAddedInRole = await _unitOfWork.UserRepository.AddToRoleAsync(aspUser, new List<string> { "admin"});
                else
                    isUserAddedInRole = await _unitOfWork.UserRepository.AddToRoleAsync(aspUser, new List<string> { "appUser" });

                if (!isUserAddedInRole)
                {
                    await _unitOfWork.UserRepository.DeleteAspUserAsync(aspUser);
                    return null;
                }

                appUser.Aspnetuserid = aspUser.Id;

                if(user.ProfileImage  != null)
                {
                    string? url = await _cloudinaryService.UploadImage(user.ProfileImage);
                    if(url == null)
                    {
                        await _unitOfWork.UserRepository.DeleteAspUserAsync(aspUser);
                        return null;
                    }
                    appUser.Profileimage = url;
                }

                bool isAppUserAdded = _unitOfWork.UserRepository.AddAppUserAsync(appUser);

                if (!isAppUserAdded)
                {
                    await _unitOfWork.UserRepository.DeleteAspUserAsync(aspUser);
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

        public async Task<Users> AddUserWithoutPasswordAsync(UserRegistrationDto user)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var appUser = _mapper.Map<Users>(user);

                //Adding asp user
                IdentityUser aspUser = new IdentityUser();

                aspUser.UserName = user.Email;
                aspUser.Email = user.Email;

                bool isAspUserAdded = await _unitOfWork.UserRepository.AddWithoutPasswordAsync(aspUser);

                if (!isAspUserAdded) return null;

                bool isUserAddedInRole = await _unitOfWork.UserRepository.AddToRoleAsync(aspUser, new List<string> { "appUser" });

                if (!isUserAddedInRole)
                {
                    await _unitOfWork.UserRepository.DeleteAspUserAsync(aspUser);
                    return null;
                }

                appUser.Aspnetuserid = aspUser.Id;

                bool isAppUserAdded = _unitOfWork.UserRepository.AddAppUserAsync(appUser);

                if (!isAppUserAdded)
                {
                    await _unitOfWork.UserRepository.DeleteAspUserAsync(aspUser);
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
        public async Task<bool> CheckPasswordAsync(Users user, string password)
        {
            try
            {
                var identityUser = await _unitOfWork.UserRepository.GetIdentityUser(user.Aspnetuserid);
                return await _unitOfWork.UserRepository.CheckPasswordAsync(identityUser, password);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<List<string>?> GetUserRoles(Users user)
        {
            var identityUser = await _unitOfWork.UserRepository.GetIdentityUser(user.Aspnetuserid);
            return await _unitOfWork.UserRepository.GetUserRolesAsync(identityUser);
        }

        public async Task ChangeUserPasswordAsync(Users user, ChangePasswordDto model)
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
                var identityUser = await _unitOfWork.UserRepository.GetIdentityUser(user.Aspnetuserid);

                await _unitOfWork.UserRepository.ChangePasswordAsync(identityUser, model.CurrentPassword, model.NewPassword);
                await _unitOfWork.SaveChangesAsync();
                return;

            }
            catch (Exception ex)
            {
                throw;
            }

        }

       

        public async Task<Users?> FindUserAsync(string email)
        {
            return await _unitOfWork.UserRepository.FindByEmailAsync(email);
        }

        public async Task<Users?> FindUserById(int Id)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(Id);
            return user;
        }

        /* public async Task<UserProfileDto> GetUserProfileAsync(string email)
         {
             try
             {
                 var user = await _unitOfWork.UserRepository.FindByEmailAsync(email);

                 if (user == null) return null;

                 var userRoles = await _unitOfWork.UserRepository.GetUserRolesAsync(user);

                 UserProfileDto userProfile = _mapper.Map<UserProfileDto>(user);
                 userProfile.Roles = userRoles;

                 return userProfile;
             }
             catch (Exception ex)
             {
                 throw;
             }
         }*/












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
