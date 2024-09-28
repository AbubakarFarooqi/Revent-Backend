using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.DataAccess.Implementation.DbContexts
{
    public class ReventDbContext : DbContext
    {
        public ReventDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
