using ClinicManagementSystem_UWU.Interfaces;
using ClinicManagementSystem_UWU.Models.Auth;
using ClinicManagementSystem_UWU.Models.Data;
using ClinicManagementSystem_UWU.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ClinicManagementSystem_UWU.Services
{
    public class AppointmentRequestService : IAppointmentRequestService
    {
        private readonly ClinicDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppointmentRequestService(ClinicDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<AppointmentRequestDto>> GetAllAsync(string? status)
        {
            var users = _httpContextAccessor.HttpContext?.User.Identity.Name;

            // Retrieve the user from the database using the Username
            var user = _context.PatientDetails.FirstOrDefault(u => u.User.Username == users);

            // If the user is not found, return all appointments without filtering by patient
            if (user == null)
            {
                return await _context.Set<AppointmentRequest>()
                    .Where(a => string.IsNullOrEmpty(status) || a.Status == status)
                    .Select(a => new AppointmentRequestDto
                    {
                        Id = a.Id,
                        PatientId = a.PatientId,
                        clinicId = a.CliniId,
                        AppointmentDate = a.AppointmentDate,
                        RequestedDate = a.RequestedDate,
                        ApproveUser = a.ApproveUser,
                        Status = a.Status,
                        RequestingReason = a.RequestingReason,
                        ApprovedReason = a.ApprovedReason
                    }).ToListAsync();
            }

            // If user is found, filter by the user's PatientId
            return await _context.Set<AppointmentRequest>()
                .Where(a => a.PatientId == user.PatientDetailsId &&
                            (string.IsNullOrEmpty(status) || a.Status == status))
                .Select(a => new AppointmentRequestDto
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    clinicId = a.CliniId,
                    AppointmentDate = a.AppointmentDate,
                    RequestedDate = a.RequestedDate,
                    ApproveUser = a.ApproveUser,
                    Status = a.Status,
                    RequestingReason = a.RequestingReason,
                    ApprovedReason = a.ApprovedReason
                }).ToListAsync();
        }


        public async Task<AppointmentRequestDto> GetByIdAsync(int id)
        {
            var appointment = await _context.Set<AppointmentRequest>().FindAsync(id);
            if (appointment == null) return null;
            return new AppointmentRequestDto
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                clinicId = appointment.CliniId,
                AppointmentDate = appointment.AppointmentDate,
                RequestedDate = appointment.RequestedDate,
                ApproveUser = appointment.ApproveUser,
                Status = appointment.Status,
                RequestingReason = appointment.RequestingReason,
                ApprovedReason = appointment.ApprovedReason
            };
        }

        public async Task<AppointmentRequestDto> CreateAsync(CreateAppointmentRequestDto request)
        {
            var users = _httpContextAccessor.HttpContext?.User.Identity.Name;

            var patient = await _context.PatientDetails
                .Where(u => u.User.Username == users)
                .Select(u => u.PatientDetailsId)
                .FirstOrDefaultAsync(); 

            
            if (patient == 0)
            {
                throw new UnauthorizedAccessException("Patient not found.");
            }

            
            var appointment = new AppointmentRequest
            {
                PatientId = patient,  
                CliniId = request.CliniId,
                AppointmentDate = Convert.ToDateTime(request.AppointmentDate),
                RequestedDate = DateTime.Now,
                Status = "Pending",
                RequestingReason = request.RequestingReason,
            };

            // Add the new appointment to the context and save changes
            _context.Set<AppointmentRequest>().Add(appointment);
            await _context.SaveChangesAsync();

            // Return the appointment DTO
            return new AppointmentRequestDto
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                clinicId = appointment.CliniId,
                AppointmentDate = appointment.AppointmentDate,
                RequestedDate = appointment.RequestedDate,
                ApproveUser = appointment.ApproveUser,
                Status = appointment.Status,
                RequestingReason = appointment.RequestingReason,
                ApprovedReason = appointment.ApprovedReason
            };
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var appointment = await _context.Set<AppointmentRequest>().FindAsync(id);
            if (appointment == null) return false;
            _context.Set<AppointmentRequest>().Remove(appointment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
