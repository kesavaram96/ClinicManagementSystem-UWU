using ClinicManagementSystem_UWU.Models.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class NurseDetails
{
    [Key]
    public int NurseDetailsId { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }
    public string Shift { get; set; }
    public string Specialization { get; set; }
    public string Ward { get; set; }
}
