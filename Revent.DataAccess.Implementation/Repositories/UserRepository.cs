using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Revent.DataAccess.Implementation.DbContexts;
using Revent.DataAccess.Implementation.IRepositories;
using Revent.EFCore.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.DataAccess.Implementation.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ReventDbContext _dbContext;

        public UserRepository(UserManager<IdentityUser> userManager, ReventDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }
        public async Task<bool> AddAspUserAsync(IdentityUser user, string password)
        {
            return (await _userManager.CreateAsync(user, password)).Succeeded;
        }

        public async Task<bool> AddToRoleAsync(IdentityUser user, List<string> roles)
        {
            try
            {
                return (await _userManager.AddToRolesAsync(user, roles)).Succeeded;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AddAppUserAsync(Users user)
        {
            try
            {
                _dbContext.Users.Add(user);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddWithoutPasswordAsync(IdentityUser user)
        {
            var identityResult = await _userManager.CreateAsync(user);
            return identityResult.Succeeded;
        }

        public async Task<Users?> FindByEmailAsync(string email)
        {
            return await _dbContext.Users
                .Include(u => u.Aspnetuser)
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CheckPasswordAsync(IdentityUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<List<string>> GetUserRolesAsync(IdentityUser user)
        {
            return (List<string>)await _userManager.GetRolesAsync(user);
        }

        public async Task ChangePasswordAsync(IdentityUser user, string currentPassword, string newPassword)
        {
            await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }






        //public async Task<bool> RemoveFromRoleAsync(ApplicationUser user, List<string> roles)
        //{
        //    return (await _userManager.RemoveFromRolesAsync(user, roles)).Succeeded;
        //}
        public async Task DeleteAspUserAsync(IdentityUser user)
        {
            await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityUser?> GetIdentityUser(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }




    }
}
