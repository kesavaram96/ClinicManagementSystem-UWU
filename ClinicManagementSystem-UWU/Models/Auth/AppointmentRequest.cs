using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem_UWU.Models.Auth
{
    public class AppointmentRequest
    {
        
            [Key]
            public int Id { get; set; }

            [ForeignKey("Patient")]
            public int PatientId { get; set; }
            public PatientDetails Patient { get; set; } // Change to PatientDetails

            [ForeignKey("Clinic")]
            public int CliniId { get; set; }
            public Clinic Clinic { get; set; }

            public DateTime AppointmentDate { get; set; }
            public DateTime RequestedDate { get; set; } = DateTime.Now;
            public string? ApproveUser { get; set; }
            public string? Status { get; set; } //pending approved rejected
            public string? RequestingReason { get; set; } //rejected reason

            public string? ApprovedReason { get; set; } //approved/rejected reason
    }
    }
