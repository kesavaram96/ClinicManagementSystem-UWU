namespace ClinicManagementSystem_UWU.Models.DTO
{
    public class PersonalInformationInputDto
    {
        //public int UserId { get; set; }
        public string Role { get; set; } // Admin, Doctor, Nurse, Receptionist, Patient

        // Admin-specific
        public string Department { get; set; }
        public string Position { get; set; }

        // Doctor-specific
        public string Specialization { get; set; }
        public DateTime? JoiningDate { get; set; }

        // Nurse-specific
        public string Shift { get; set; }
        public string Ward { get; set; }

        // Receptionist-specific
        public string DeskLocation { get; set; }
        public string AssignedDoctor { get; set; }

        // Patient-specific
        public string MedicalHistory { get; set; }
        public string InsuranceNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string EmergencyContactPerson { get; set; }
        public string ECNumber { get; set; }
        public string ECRelationship { get; set; }
        public string BloodGroup { get; set; }
    }
}