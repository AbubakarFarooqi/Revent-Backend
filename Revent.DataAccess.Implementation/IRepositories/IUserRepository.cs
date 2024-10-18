using Microsoft.AspNetCore.Identity;
using Revent.EFCore.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.DataAccess.Implementation.IRepositories
{
    public interface IUserRepository : IBaseRepository<Users>
    {
        Task<bool> AddAspUserAsync(IdentityUser user, string password);
        bool AddAppUserAsync(Users user);
        Task<bool> AddToRoleAsync(IdentityUser user, List<string> roles);

        Task<bool> AddWithoutPasswordAsync(IdentityUser user);
        Task<Users?> FindByEmailAsync(string email);
        Task<List<string>> GetUserRolesAsync(IdentityUser user);
        //Task<bool> RemoveFromRoleAsync(ApplicationUser user, List<string> roles);
        Task ChangePasswordAsync(IdentityUser user, string currentPassword, string newPassword);
        Task<bool> CheckPasswordAsync(IdentityUser user, string password);
        Task DeleteAspUserAsync(IdentityUser user);

        Task<IdentityUser> GetIdentityUser(string userId);
    }
}
