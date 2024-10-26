using ClinicManagementSystem_UWU.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem_UWU.Models.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ClinicDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ClinicDbContext>>());

            // Look for any roles already in the database.
            if (context.Roles.Any()) return; // Database has been seeded

            context.Roles.AddRange(
                new Role { RoleName = "Admin", Description = "Full access to the system" },
                new Role { RoleName = "Doctor", Description = "Access to patient records and appointment management" },
                new Role { RoleName = "Nurse", Description = "Access to patient care-related information" },
                new Role { RoleName = "Receptionist", Description = "Access to scheduling and basic patient info" }
            );

            context.SaveChanges();
        }
    }

}
