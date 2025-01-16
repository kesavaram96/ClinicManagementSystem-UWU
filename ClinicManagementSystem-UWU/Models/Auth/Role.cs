using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem_UWU.Models.Auth
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }

        // Navigation property for the users in this role
        public ICollection<UserRole> UserRoles { get; set; }
    }

}
