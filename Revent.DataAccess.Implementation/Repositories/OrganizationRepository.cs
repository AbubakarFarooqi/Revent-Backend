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
    public class OrganizationRepository : BaseRepository<Organizations>, IOrganizationRepository
    {

        private readonly ReventDbContext _dbContext;
        public OrganizationRepository(ReventDbContext applicationDbContext) : base(applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<Organizations> GetByUserIdAsync(int userId)
        {
            var organization = await _dbContext.Organizations.FirstOrDefaultAsync(x => x.UserId == userId);
            return organization;
        }
    }
}
