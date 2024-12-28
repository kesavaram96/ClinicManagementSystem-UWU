using ClinicManagementSystem_UWU.Models.Auth;

public class AdminDetails
{
    public int AdminDetailsId { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public string Department { get; set; }
    public string Position { get; set; }
}
