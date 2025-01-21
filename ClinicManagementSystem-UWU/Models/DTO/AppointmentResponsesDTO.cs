namespace ClinicManagementSystem_UWU.Models.DTO
{
    public class AppointmentResponsesDTO
    {
        public int AppointmentId { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public string ClinicName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }
        public int LineNumber { get; set; }
        public int? DoctorId { get; set; }  // Add DoctorId
        public int ClinicId { get; set; }  // Add ClinicId
        public int PatientId { get; set; }  // Add PatientId
    }

}
