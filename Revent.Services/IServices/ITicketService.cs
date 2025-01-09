using Revent.Common.CommonDtos;
using Revent.EFCore.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.Services.IServices
{
    public interface ITicketService
    {
        List<Tickets> GetAllByEventIdAsync(int id);
        Task<Tickets> GetByIdAsync(int id);
        Task CreateTicketAsync(TicketDto ticketDto);

        Task DeleteAsync(int id);
        Task UpdateTicketAsync(int id,TicketDto ticketUpdateDto);
    }
}
