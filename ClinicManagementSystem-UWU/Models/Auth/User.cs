using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ClinicManagementSystem_UWU.Models.Auth
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string? FullName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property for the roles
        //[JsonIgnore]
        
        public ICollection<UserRole> UserRoles { get; set; }
    }

}
