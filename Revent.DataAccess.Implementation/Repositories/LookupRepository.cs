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
    public class LookupRepository : BaseRepository<Lookups>, ILookupRepository
    {
        private readonly ReventDbContext _dbContext;
        public LookupRepository(ReventDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
