using Revent.Common.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.DataAccess.Implementation.IRepositories
{
    public interface IUserRepository 
    {
        Task<bool> AddAsync(ApplicationUser user, string password);
        Task<bool> AddWithoutPasswordAsync(ApplicationUser user);
        Task<bool> AddToRoleAsync(ApplicationUser user, List<string> roles);
        Task<ApplicationUser?> FindAsync(string username);
        Task<List<string>> GetRoleAsync(ApplicationUser user);
        Task<bool> RemoveFromRoleAsync(ApplicationUser user, List<string> roles);
        Task ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task DeleteAsync(ApplicationUser user);
    }
}
