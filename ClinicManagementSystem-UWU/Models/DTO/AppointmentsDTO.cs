namespace ClinicManagementSystem_UWU.Models.DTO
{
    public class AppointmentsDTO
    {
        public int DoctorId { get; set; }  // DoctorId
        public int ClinicId { get; set; }  // ClinicId
        public string Username { get; set; }  // Username of the patient
        public DateTime AppointmentDate { get; set; }  // Appointment date
        public string Status { get; set; }  // Status (e.g., "Scheduled", "Canceled")
    }


}

