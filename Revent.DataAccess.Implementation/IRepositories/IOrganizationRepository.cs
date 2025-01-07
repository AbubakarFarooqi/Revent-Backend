using Revent.EFCore.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.DataAccess.Implementation.IRepositories
{
    public interface IOrganizationRepository:IBaseRepository<Organizations>
    {
        Task<Organizations> GetByUserIdAsync(int userId);
    }
}
