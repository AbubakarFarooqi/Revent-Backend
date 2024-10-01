using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revent.Common.CommonModels;

namespace Revent.DataAccess.Implementation.DbContexts
{
    public class AuthDbContext:IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
