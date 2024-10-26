namespace ClinicManagementSystem_UWU.Models.Auth
{
    public class UserRole
    {
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public DateTime AssignedDate { get; set; } = DateTime.Now;
    }

}
