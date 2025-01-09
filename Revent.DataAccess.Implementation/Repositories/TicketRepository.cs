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
    public class TicketRepository:BaseRepository<Tickets>,ITicketRepository
    {
        private readonly ReventDbContext _dbContext;

        public TicketRepository(ReventDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public List<Tickets> GetByEventIdAsync(int id)
        {
           var tickets =  _dbContext.Tickets.Where(x => x.EventId == id).ToList();
            return tickets;
        }
    }
}
