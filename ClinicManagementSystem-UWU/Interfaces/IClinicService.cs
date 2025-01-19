using ClinicManagementSystem_UWU.Models.DTO;

namespace ClinicManagementSystem_UWU.Interfaces
{
    public interface IClinicService
    {
        Task<string> CreateClinicAsync(ClinicDTO clinicDto);
        Task<string> UpdateClinicAsync(ClinicDTO clinicDto);
        Task<string> DeleteClinicAsync(int clinicId);
        Task<List<ClinicDTO>> GetAllClinicsAsync();
        Task<ClinicDTO> GetClinicByIdAsync(int clinicId);
    }
}
