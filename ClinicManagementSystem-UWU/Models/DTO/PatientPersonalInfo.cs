using ClinicManagementSystem_UWU.Models.Auth;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem_UWU.Models.DTO
{
    public class PatientPersonalInfo
    {

        public string? MedicalHistory { get; set; }
        public string? InsuranceNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public string? EmergencyContactPerson { get; set; }
        public string? ECNumber { get; set; }
        public string? ECRelationship { get; set; }
        public string? BloodGroup { get; set; }
        public string? Gender { get; set; }

    }

}
