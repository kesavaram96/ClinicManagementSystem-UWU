using ClinicManagementSystem_UWU.Models.Auth;

public class ReceptionistDetails
{
    public int ReceptionistDetailsId { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public string DeskLocation { get; set; }
    public string AssignedDoctor { get; set; }
    public string Shift { get; set; }
}
