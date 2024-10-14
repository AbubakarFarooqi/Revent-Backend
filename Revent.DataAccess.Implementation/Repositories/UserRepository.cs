using Microsoft.AspNetCore.Identity;
using Revent.Common.CommonModels;
using Revent.DataAccess.Implementation.DbContexts;
using Revent.DataAccess.Implementation.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.DataAccess.Implementation.Repositories
{
    public class UserRepository :IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public  async Task<bool> AddAsync(ApplicationUser user, string password)
        {
            return (await _userManager.CreateAsync(user, password)).Succeeded;
        }

        public async Task<bool> AddToRoleAsync(ApplicationUser user, List<string> roles)
        {
            try
            {
                return (await _userManager.AddToRolesAsync(user, roles)).Succeeded;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddWithoutPasswordAsync(ApplicationUser user)
        {
            return (await _userManager.CreateAsync(user)).Succeeded;
        }

        public async Task ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
        {
            await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<ApplicationUser?> FindAsync(string username)
        {
            return await _userManager.FindByEmailAsync(username);
        }

        public async Task<List<string>> GetUserRolesAsync(ApplicationUser user)
        {
           return (List<string>)await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> RemoveFromRoleAsync(ApplicationUser user, List<string> roles)
        {
            return (await _userManager.RemoveFromRolesAsync(user, roles)).Succeeded;
        }
        public  async Task DeleteAsync(ApplicationUser user)
        {
            await _userManager.DeleteAsync(user);
        }
    }
}
