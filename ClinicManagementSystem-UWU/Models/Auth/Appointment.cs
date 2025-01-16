using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagementSystem_UWU.Models.Auth
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public PatientDetails Patient { get; set; }

        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public DoctorDetails Doctor { get; set; }

        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public Room Room { get; set; }

        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } // Scheduled, Completed, Cancelled
        public int LineNumber { get; set; } // Queue number for the room
    }
}
