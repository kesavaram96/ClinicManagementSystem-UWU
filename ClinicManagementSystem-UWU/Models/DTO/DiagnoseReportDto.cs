namespace ClinicManagementSystem_UWU.Models.DTO
{
    public class DiagnoseReportDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int PatientId { get; set; }
        public string? HeartRate { get; set; }
        public decimal? Weight { get; set; }
        public string? Diagnose { get; set; }
        public string? Medicines { get; set; }
        public string? HealthStatus { get; set; }
    }
}
