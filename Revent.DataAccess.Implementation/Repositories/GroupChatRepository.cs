using Microsoft.EntityFrameworkCore;
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
    public class GroupChatRepository : BaseRepository<GroupChats>, IGroupChatRepository
    {
        ReventDbContext _dbContext;
        public GroupChatRepository(ReventDbContext applicationDbContext) : base(applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public GroupChats? GetAsync(int id)
        {
            return  _dbContext.GroupChats.Where(x=> x.Id == id)
                    .Include(x => x.Participants)
                        .ThenInclude(u=>u.User)
                    .FirstOrDefault();


        }
    }
}
