using ClinicManagementSystem_UWU.Interfaces;
using ClinicManagementSystem_UWU.Models.Auth;
using ClinicManagementSystem_UWU.Models.Data;
using ClinicManagementSystem_UWU.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System;

namespace ClinicManagementSystem_UWU.Services
{
    public class DiagnoseReportService : IDiagnoseReportService
    {
        private readonly ClinicDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DiagnoseReportService(ClinicDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<DiagnoseReportDto>> GetDiagnoseReports(int? patientId)
        {
            if (patientId == null)
            {
                var username = _httpContextAccessor.HttpContext?.User.Identity.Name;
                var user = await _context.PatientDetails.Include(p => p.User)
                                                        .FirstOrDefaultAsync(u => u.User.Username == username);
                if (user != null)
                {
                    patientId = user.PatientDetailsId;
                }
                else
                {
                    throw new Exception("Patient ID is required or user must be a registered patient.");
                }
            }

            return await _context.DiagnoseReport
                .Where(d => d.PatientId == patientId)
                .Select(d => new DiagnoseReportDto
                {
                    Id = d.Id,
                    Date = d.Date,
                    PatientId = d.PatientId,
                    HeartRate = d.HeartRate,
                    Weight = d.Weight,
                    Diagnose = d.Diagnose,
                    Medicines = d.Medicines,
                    HealthStatus = d.HealthStatus
                })
                .ToListAsync();
        }

        public async Task<DiagnoseReportDto> CreateDiagnoseReport(DiagnoseReportDto reportDto)
        {
            var username = _httpContextAccessor.HttpContext?.User.Identity.Name;
            var doctor = await _context.DoctorDetails.Include(d => d.User)
                                                     .FirstOrDefaultAsync(d => d.User.Username == username);

            if (doctor == null)
            {
                throw new Exception("Only registered doctors can create diagnose reports.");
            }

            var report = new DiagnoseReport
            {
                Date = DateTime.Now,
                PatientId = reportDto.PatientId,
                HeartRate = reportDto.HeartRate,
                Weight = reportDto.Weight,
                Diagnose = reportDto.Diagnose,
                Medicines = reportDto.Medicines,
                HealthStatus = reportDto.HealthStatus
            };

            _context.DiagnoseReport.Add(report);
            await _context.SaveChangesAsync();

            reportDto.Id = report.Id;
            return reportDto;
        }
    }
}