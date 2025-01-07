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

    public virtual DbSet<Events> Events { get; set; }

    public virtual DbSet<GroupChats> GroupChats { get; set; }

    public virtual DbSet<GroupMessages> GroupMessages { get; set; }

    public virtual DbSet<Lookups> Lookups { get; set; }

    public virtual DbSet<OpenIddictApplications> OpenIddictApplications { get; set; }

    public virtual DbSet<OpenIddictAuthorizations> OpenIddictAuthorizations { get; set; }

    public virtual DbSet<OpenIddictScopes> OpenIddictScopes { get; set; }

    public virtual DbSet<OpenIddictTokens> OpenIddictTokens { get; set; }

    public virtual DbSet<Organizations> Organizations { get; set; }

    public virtual DbSet<Participants> Participants { get; set; }

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

        modelBuilder.Entity<Events>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("events_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.EventTypeNavigation).WithMany(p => p.Events)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_event_type");

            entity.HasOne(d => d.GroupChat).WithMany(p => p.Events)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_group_chat");

            entity.HasOne(d => d.Organizer).WithMany(p => p.Events).HasConstraintName("fk_organizer");
        });

        modelBuilder.Entity<GroupChats>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("group_chats_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<GroupMessages>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("group_messages_pkey");

            entity.Property(e => e.SentAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.GroupChat).WithMany(p => p.GroupMessages).HasConstraintName("fk_group_chat_msg");

            entity.HasOne(d => d.Participant).WithMany(p => p.GroupMessages).HasConstraintName("fk_participant");
        });

        modelBuilder.Entity<Lookups>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("lookups_pkey");
        });

        modelBuilder.Entity<OpenIddictAuthorizations>(entity =>
        {
            entity.HasOne(d => d.Application).WithMany(p => p.OpenIddictAuthorizations).HasConstraintName("FK_OpenIddictAuthorizations_OpenIddictApplications_Application~");
        });

        modelBuilder.Entity<Organizations>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("organizations_pkey");

            entity.HasOne(d => d.ContactNumberNavigation).WithMany(p => p.OrganizationsContactNumberNavigation)
                .HasPrincipalKey(p => p.PhoneNumber)
                .HasForeignKey(d => d.ContactNumber)
                .HasConstraintName("organizations_contact_number_fkey");

            entity.HasOne(d => d.OrganizationTypeNavigation).WithMany(p => p.Organizations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("organizations_organization_type_fkey");

            entity.HasOne(d => d.User).WithOne(p => p.OrganizationsUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("organizations_user_id_fkey");
        });

        modelBuilder.Entity<Participants>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("participants_pkey");

            entity.Property(e => e.JoinedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.GroupChat).WithMany(p => p.Participants).HasConstraintName("fk_group_chat");

            entity.HasOne(d => d.User).WithMany(p => p.Participants).HasConstraintName("fk_user");
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
