namespace ClinicManagementSystem_UWU.Models.DTO
{
    public class UserListDTO
    {
        
            public int UserId { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public List<string> RoleNames { get; set; } // Only include role names, not full navigation
        

    }
}
