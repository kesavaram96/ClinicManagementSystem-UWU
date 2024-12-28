using ClinicManagementSystem_UWU.Models.Auth;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ClinicManagementSystem_UWU.Models.Data
{
    public class ClinicDbContext:DbContext
    {
            public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options) { }

        //Auth tables
            public DbSet<User> Users { get; set; }
            public DbSet<Role> Roles { get; set; }
            public DbSet<UserRole> UserRoles { get; set; }

        //Information Tables
        public DbSet<DoctorDetails> DoctorDetails { get; set; }
        public DbSet<PatientDetails> PatientDetails { get; set; }
        public DbSet<AdminDetails> AdminDetails { get; set; }
        public DbSet<NurseDetails> NurseDetails { get; set; }
        public DbSet<ReceptionistDetails> ReceptionistDetails { get; set; }

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
