using ClinicManagementSystem_UWU.Models.Auth;
using System.ComponentModel.DataAnnotations.Schema;

public class PatientDetails
{
    public int PatientDetailsId { get; set; } 
    [ForeignKey("User")]
    public int UserId { get; set; } 
    public User User { get; set; } 
    public string MedicalHistory { get; set; }
    public string InsuranceNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }

    public string EmergencyContactPerson { get; set; }
    public string ECNumber { get; set; }
    public string ECRelationship { get; set; }
    public string BloodGroup { get; set; }

}
