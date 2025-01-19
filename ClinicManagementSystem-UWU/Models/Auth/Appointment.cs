using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagementSystem_UWU.Models.Auth
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public PatientDetails Patient { get; set; } // Change to PatientDetails

        // Doctor Foreign Key
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public DoctorDetails Doctor { get; set; }

        [ForeignKey("Clinic")]
        public int CliniId { get; set; }
        public Clinic Clinic { get; set; }

        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } // Scheduled, Completed, Cancelled
        public int LineNumber { get; set; } // Queue number for the room
    }
}
