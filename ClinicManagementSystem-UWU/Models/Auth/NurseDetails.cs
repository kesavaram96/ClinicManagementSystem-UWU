using ClinicManagementSystem_UWU.Models.Auth;

public class NurseDetails
{
    public int NurseDetailsId { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public string Shift { get; set; }
    public string Specialization { get; set; }
    public string Ward { get; set; }
}
