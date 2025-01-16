using ClinicManagementSystem_UWU.Models.Auth;
using System.ComponentModel.DataAnnotations.Schema;

public class ReceptionistDetails
{
    public int ReceptionistDetailsId { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }
    public string DeskLocation { get; set; }
    public string AssignedDoctor { get; set; }
    public string Shift { get; set; }
}
