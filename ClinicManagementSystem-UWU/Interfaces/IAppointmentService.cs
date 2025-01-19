using ClinicManagementSystem_UWU.Models.DTO;

namespace ClinicManagementSystem_UWU.Interfaces
{
    public interface IAppointmentService
    {
        Task<List<AppointmentResponsesDTO>> GetAllAppointmentsAsync();
        Task<AppointmentResponsesDTO> GetAppointmentByIdAsync(int appointmentId);
        Task<AppointmentResponseDTO> UpdateAppointmentAsync(int appointmentId, AppointmentsDTO appointmentDto);
        Task<string> DeleteAppointmentAsync(int appointmentId);
        Task<AppointmentResponseDTO> BookAppointmentAsync(AppointmentDTO appointmentDto);
        Task<IEnumerable<ClinicDTO>> GetAllClinicsAsync();
        Task<ClinicDTO> GetClinicByIdAsync(int clinicId);

        Task<IEnumerable<DoctorDTO>> GetAllDoctorsAsync();
        Task<DoctorDTO> GetDoctorByIdAsync(int doctorId);
    }
}
