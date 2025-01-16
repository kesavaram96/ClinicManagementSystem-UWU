using ClinicManagementSystem_UWU.Models.Auth;
using System.ComponentModel.DataAnnotations.Schema;

public class DoctorDetails
{
    public int DoctorDetailsId { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; } 
    public User User { get; set; } 
    public string Specialization { get; set; }
    public DateTime JoiningDate { get; set; }

    
}
