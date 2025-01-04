using Revent.EFCore.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.DataAccess.Implementation.IRepositories
{
    public interface IEventRepository:IBaseRepository<Events>
    {
        Task<Events?> DeleteEvent(int id);
    }
}
