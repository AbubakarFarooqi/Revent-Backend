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
    public class EventRepository : BaseRepository<Events>, IEventRepository
    {
        ReventDbContext _dbContext;
        public EventRepository(ReventDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
