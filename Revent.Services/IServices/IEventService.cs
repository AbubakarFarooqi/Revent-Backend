using Revent.Common.CommonDtos;
using Revent.EFCore.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.Services.IServices
{
    public interface IEventService
    {
        Task AddEventAsync(EventCreateDto eventCreateDto );
        Task<Events> GetEventByIdAsync(int id );
    }
}
