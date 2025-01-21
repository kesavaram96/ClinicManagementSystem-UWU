using ClinicManagementSystem_UWU.Models.DTO;
namespace ClinicManagementSystem_UWU.Interfaces
{
    public interface IAppointmentRequestService
    {
        Task<List<AppointmentRequestDto>> GetAllAsync(string status);
        Task<AppointmentRequestDto> GetByIdAsync(int id);
        Task<AppointmentRequestDto> CreateAsync(CreateAppointmentRequestDto request);
        Task<bool> DeleteAsync(int id);
        Task<AppointmentResponseDTO> UpdateAppointmentRequestStatusAsync(int requestId, string status, string approvedReason);

    }
}
