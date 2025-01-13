using ClinicManagementSystem_UWU.Models.Auth;

public class DoctorDetails
{
    public int DoctorDetailsId { get; set; } 
    public int UserId { get; set; } 
    public User User { get; set; } 
    public string Specialization { get; set; }
    public DateTime JoiningDate { get; set; }

    
}
