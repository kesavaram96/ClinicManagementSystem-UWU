using ClinicManagementSystem_UWU.Interfaces;
using ClinicManagementSystem_UWU.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem_UWU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnoseReportController : ControllerBase
    {
        private readonly IDiagnoseReportService _diagnoseReportService;

        public DiagnoseReportController(IDiagnoseReportService diagnoseReportService)
        {
            _diagnoseReportService = diagnoseReportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDiagnoseReports([FromQuery] int? patientId)
        {
            try
            {
                var reports = await _diagnoseReportService.GetDiagnoseReports(patientId);
                return Ok(reports);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateDiagnoseReport([FromBody] DiagnoseReportDto reportDto)
        {
            try
            {
                var createdReport = await _diagnoseReportService.CreateDiagnoseReport(reportDto);
                return CreatedAtAction(nameof(GetDiagnoseReports), new { patientId = createdReport.PatientId }, createdReport);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
