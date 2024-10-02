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
    public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(ReventDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
            : base(applicationDbContext)
        {
            _userManager = userManager;
        }
        public  async Task AddAsync(ApplicationUser user, string password)
        {
            await _userManager.CreateAsync(user, password);
        }

        public async Task AddToRoleAsync(ApplicationUser user, List<string> roles)
        {
            await _userManager.AddToRolesAsync(user, roles);
        }

        public async Task AddWithoutPasswordAsync(ApplicationUser user)
        {
            await _userManager.CreateAsync(user);
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

        public async Task<List<string>> GetRoleAsync(ApplicationUser user)
        {
            return (List<string>) await _userManager.GetRolesAsync(user);
        }

        public async Task RemoveFromRoleAsync(ApplicationUser user, List<string> roles)
        {
            await _userManager.RemoveFromRolesAsync(user, roles);
        }
    }
}
