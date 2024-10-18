using Revent.EFCore.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.Services.IServices
{
    public interface ILookupService
    {
        Task<Lookups?> GetLookupById(int Id);
    }
}
