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
    public class GroupMessageRepository : BaseRepository<GroupMessages>, IGroupMessageRepository
    {
        ReventDbContext _dbContext;
        public GroupMessageRepository(ReventDbContext applicationDbContext) : base(applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }
    }
}
