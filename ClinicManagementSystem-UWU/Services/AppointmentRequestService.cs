using ClinicManagementSystem_UWU.Interfaces;
using ClinicManagementSystem_UWU.Models.Auth;
using ClinicManagementSystem_UWU.Models.Data;
using ClinicManagementSystem_UWU.Models.DTO;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ClinicManagementSystem_UWU.Services
{
    public class AppointmentRequestService : IAppointmentRequestService
    {
        private readonly ClinicDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private AppointmentService appointment;
        private readonly IHubContext<NotificationHub> _hubContext;
        public AppointmentRequestService(ClinicDbContext context, IHttpContextAccessor httpContextAccessor, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
            appointment = new AppointmentService(_context, _hubContext);
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
                        PatientName=a.Patient.User.FullName,
                        clinicId = a.CliniId,
                        CLinicName=a.Clinic.ClinicName,
                        AppointmentDate = a.AppointmentDate,
                        RequestedDate = a.RequestedDate,
                        ApproveUser = a.ApproveUser,
                        Status = a.Status,
                        RequestingReason = a.RequestingReason,
                        ApprovedReason = a.ApprovedReason,
                        DoctorId=a.DoctorId,
                        DoctorName=a.Doctor.User.FullName
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
                    PatientName = a.Patient.User.FullName,
                    clinicId = a.CliniId,
                    CLinicName = a.Clinic.ClinicName,
                    AppointmentDate = a.AppointmentDate,
                    RequestedDate = a.RequestedDate,
                    ApproveUser = a.ApproveUser,
                    Status = a.Status,
                    RequestingReason = a.RequestingReason,
                    ApprovedReason = a.ApprovedReason,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.User.FullName
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

            var doc = await _context.DoctorDetails.Where(u => u.DoctorDetailsId == request.DoctorId).Select(u=>u.DoctorDetailsId).FirstOrDefaultAsync();

            if(doc==0)
            {
                throw new UnauthorizedAccessException("Doctor not found.");
            }
            
            if (patient == 0)
            {
                throw new UnauthorizedAccessException("Patient not found.");
            }

            
            var appointment = new AppointmentRequest
            {
                PatientId = patient,  
                CliniId = request.clinicId,
                AppointmentDate = Convert.ToDateTime(request.AppointmentDate),
                RequestedDate = DateTime.Now,
                Status = "Pending",
                RequestingReason = request.RequestingReason,
                DoctorId=doc
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
                ApprovedReason = appointment.ApprovedReason,
                DoctorId=appointment.DoctorId
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

        public async Task<AppointmentResponseDTO> UpdateAppointmentRequestStatusAsync(int requestId, string status, string approvedReason)
        {
            var appointmentRequest = await _context.AppointmentRequest.FindAsync(requestId);
            if (appointmentRequest == null)
            {
                return new AppointmentResponseDTO { Message = "Appointment request not found!" };
            }

            appointmentRequest.Status = status;
            appointmentRequest.ApprovedReason = approvedReason;
            AppointmentResponseDTO appointmentResponse =null;
            if (status == "Approve")
            {
                var appointmentDto = new AppointmentDTO
                {
                    ClinicId = appointmentRequest.CliniId,
                    DoctorId = appointmentRequest.DoctorId, // Assuming ApproveUser is the doctor ID
                    AppointmentDate = appointmentRequest.AppointmentDate,
                    Username = _context.PatientDetails
                        .Where(p => p.PatientDetailsId == appointmentRequest.PatientId)
                        .Select(p => p.User.Username)
                        .FirstOrDefault()
                };

                appointmentResponse = await appointment.BookAppointmentAsync(appointmentDto);
                if (!appointmentResponse.Message.Contains("Appointment booked successfully!"))
                {
                    return appointmentResponse; // Return the error message if booking failed
                }
            }

            await _context.SaveChangesAsync();
            return appointmentResponse;
        }

    }
}
