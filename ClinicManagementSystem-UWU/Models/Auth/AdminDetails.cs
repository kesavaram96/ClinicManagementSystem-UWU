using ClinicManagementSystem_UWU.Models.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class AdminDetails
{
    [Key]
    public int AdminDetailsId { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }
    public string Department { get; set; }
    public string Position { get; set; }
}
