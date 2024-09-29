using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Revent.EFCore.DataModel.Models;

namespace Revent.DataAccess.Implementation.DbContexts;

public partial class ReventDbContext : DbContext
{
    public ReventDbContext(DbContextOptions<ReventDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Temp> Temp { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
