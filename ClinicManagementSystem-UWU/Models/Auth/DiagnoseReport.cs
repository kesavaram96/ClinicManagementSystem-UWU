using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagementSystem_UWU.Models.Auth
{
    public class DiagnoseReport
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }=DateTime.Now;
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public PatientDetails Patient { get; set; } // Change to PatientDetails
        public string? HeartRate { get; set; }
        public decimal? Weight { get; set; }
        public string? Diagnose { get; set; }

        public string? HealthStatus { get; set; }

    }
}
