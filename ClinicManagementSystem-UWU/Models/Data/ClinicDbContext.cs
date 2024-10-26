using ClinicManagementSystem_UWU.Models.Auth;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ClinicManagementSystem_UWU.Models.Data
{
    public class ClinicDbContext:DbContext
    {
            public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options) { }

            public DbSet<User> Users { get; set; }
            public DbSet<Role> Roles { get; set; }
            public DbSet<UserRole> UserRoles { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                // Configure many-to-many relationship
                modelBuilder.Entity<UserRole>()
                    .HasKey(ur => ur.UserRoleId);

                modelBuilder.Entity<UserRole>()
                    .HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId);

                modelBuilder.Entity<UserRole>()
                    .HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId);
            }
        }

    
}
