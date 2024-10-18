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

    public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }

    public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }

    public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }

    public virtual DbSet<OpenIddictApplications> OpenIddictApplications { get; set; }

    public virtual DbSet<OpenIddictAuthorizations> OpenIddictAuthorizations { get; set; }

    public virtual DbSet<OpenIddictScopes> OpenIddictScopes { get; set; }

    public virtual DbSet<OpenIddictTokens> OpenIddictTokens { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetUsers>(entity =>
        {
            entity.HasMany(d => d.Role).WithMany(p => p.User)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRoles",
                    r => r.HasOne<AspNetRoles>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUsers>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<OpenIddictAuthorizations>(entity =>
        {
            entity.HasOne(d => d.Application).WithMany(p => p.OpenIddictAuthorizations).HasConstraintName("FK_OpenIddictAuthorizations_OpenIddictApplications_Application~");
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("now()");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Aspnetuser).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_aspnetuserid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
