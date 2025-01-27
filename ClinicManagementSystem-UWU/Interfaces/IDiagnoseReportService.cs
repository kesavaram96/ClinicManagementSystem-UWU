using ClinicManagementSystem_UWU.Models.DTO;

namespace ClinicManagementSystem_UWU.Interfaces
{
    public interface IDiagnoseReportService
    {
        Task<List<DiagnoseReportDto>> GetDiagnoseReports(int? patientId);
        Task<DiagnoseReportDto> CreateDiagnoseReport(DiagnoseReportDto reportDto);

        


    }
}
