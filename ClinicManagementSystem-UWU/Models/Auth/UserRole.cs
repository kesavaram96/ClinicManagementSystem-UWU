using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagementSystem_UWU.Models.Auth
{
    public class UserRole
    {
        [Key]
        public int UserRoleId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public DateTime AssignedDate { get; set; } = DateTime.Now;
    }

}
