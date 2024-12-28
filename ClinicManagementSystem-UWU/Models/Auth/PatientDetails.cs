using ClinicManagementSystem_UWU.Models.Auth;

public class PatientDetails
{
    public int PatientDetailsId { get; set; } // Primary Key
    public int UserId { get; set; } // Foreign Key from User
    public User User { get; set; } // Navigation Property
    public string MedicalHistory { get; set; }
    public string InsuranceNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
}
