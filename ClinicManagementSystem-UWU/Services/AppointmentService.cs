using ClinicManagementSystem_UWU.Interfaces;
using ClinicManagementSystem_UWU.Models.Auth;
using ClinicManagementSystem_UWU.Models.Data;
using ClinicManagementSystem_UWU.Models.DTO;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClinicManagementSystem_UWU.Services
{
    public class AppointmentService: IAppointmentService
    {
        private readonly ClinicDbContext _context;
        private readonly Random _random;

        private readonly IHubContext<NotificationHub> _hubContext;
        public AppointmentService(ClinicDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _random = new Random();

            _hubContext = hubContext;
        }

        public async Task<List<AppointmentResponsesDTO>> GetAllAppointmentsAsync()
        {
            //await _hubContext.Clients.All.SendAsync("ReceiveNotification", new { Message = "Test Notification " + DateTime.Now });

            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .ThenInclude(p => p.User)  // Ensure User is loaded for Patient
                .Include(a => a.Doctor)
                .ThenInclude(d => d.User)  // Ensure User is loaded for Doctor
                .Include(a => a.Clinic)
                .ToListAsync();

            return appointments.Select(a => new AppointmentResponsesDTO
            {
                AppointmentId = a.AppointmentId,
                DoctorName = a.Doctor?.User?.Username ?? "Unknown Doctor",
                PatientName = a.Patient?.User?.Username ?? "Unknown Patient",
                ClinicName = a.Clinic?.ClinicName ?? "Unknown Clinic",
                AppointmentDate = a.AppointmentDate,
                Status = a.Status,
                LineNumber = a.LineNumber,
                DoctorId = a.DoctorId,  // Include DoctorId
                ClinicId = a.CliniId,  // Include ClinicId
                PatientId = a.Patient?.PatientDetailsId ?? 0  // Include PatientId
            }).ToList();
        }


        // Get appointment by ID
        public async Task<AppointmentResponsesDTO> GetAppointmentByIdAsync(int appointmentId)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .ThenInclude(p => p.User)  // Ensure User is loaded for Patient
                .Include(a => a.Doctor)
                .ThenInclude(d => d.User)  // Ensure User is loaded for Doctor
                .Include(a => a.Clinic)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (appointment == null)
            {
                return null;  // You can also throw an exception if needed.
            }

            return new AppointmentResponsesDTO
            {
                AppointmentId = appointment.AppointmentId,
                DoctorName = appointment.Doctor?.User?.Username ?? "Unknown Doctor",
                PatientName = appointment.Patient?.User?.Username ?? "Unknown Patient",
                ClinicName = appointment.Clinic?.ClinicName ?? "Unknown Clinic",
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
                LineNumber = appointment.LineNumber,
                DoctorId = appointment.DoctorId,  // Include DoctorId
                ClinicId = appointment.CliniId,  // Include ClinicId
                PatientId = appointment.Patient?.PatientDetailsId ?? 0  // Include PatientId
            };
        }


        // Update an appointment
        public async Task<AppointmentResponseDTO> UpdateAppointmentAsync(int appointmentId, AppointmentsDTO appointmentDto)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (appointment == null)
            {
                return new AppointmentResponseDTO { Message = "Appointment not found!" };
            }

            appointment.AppointmentDate = appointmentDto.AppointmentDate;
            appointment.Status = appointmentDto.Status;
            appointment.DoctorId = appointmentDto.DoctorId;  // Update DoctorId
            appointment.CliniId = appointmentDto.ClinicId;  // Update ClinicId

            await _context.SaveChangesAsync();

            return new AppointmentResponseDTO
            {
                Message = "Appointment updated successfully!",
                LineNumber = appointment.LineNumber,
                
            };
        }


        // Delete an appointment
        public async Task<string> DeleteAppointmentAsync(int appointmentId)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (appointment == null)
            {
                return "Appointment not found!";
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return "Appointment deleted successfully!";
        }
        public async Task<AppointmentResponseDTO> BookAppointmentAsync(AppointmentDTO appointmentDto)
        {

            var doctor = await _context.DoctorDetails
                .FirstOrDefaultAsync(d => d.DoctorDetailsId == appointmentDto.DoctorId);

            if (doctor == null)
            {
                return new AppointmentResponseDTO { Message = "Doctor not found!" };

            }


            var patient = await _context.PatientDetails
        .FirstOrDefaultAsync(p => p.User.Username == appointmentDto.Username);

            if (patient == null)
            {
                return new AppointmentResponseDTO
                {
                    Message = "Patient not found!"
                };
            }


            var clinic = await _context.Clinics
                .FirstOrDefaultAsync(c => c.ClinicId == appointmentDto.ClinicId);

            if (clinic == null)
            {
                return new AppointmentResponseDTO
                {
                    Message = "Clinic not found!"
                };
            }


            var existingAppointments = await _context.Appointments
                .Where(a => a.DoctorId == appointmentDto.DoctorId &&
                            a.AppointmentDate.Date == appointmentDto.AppointmentDate.Date &&
                            a.Status == "Scheduled")
                .ToListAsync();


            if (existingAppointments.Count >= clinic.PatientCapability)
            {
                return new AppointmentResponseDTO
                {
                    Message = "The clinic has reached its capacity for the day!"
                };
            }


            int randomLineNumber;
            bool isUnique;

            do
            {

                randomLineNumber = _random.Next(1, clinic.PatientCapability + 1);

                isUnique = !existingAppointments.Any(a => a.LineNumber == randomLineNumber);

            } while (!isUnique);


            var appointment = new Appointment
            {

                PatientId = patient.PatientDetailsId,
                DoctorId = appointmentDto.DoctorId,
                AppointmentDate = appointmentDto.AppointmentDate,
                Status = "Scheduled",
                LineNumber = randomLineNumber,
                CliniId = appointmentDto.ClinicId
            };

            // Save the appointment to the database
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("ReceiveNotification", new { Message = "Test Notification " + DateTime.Now });

            await _hubContext.Clients.User(patient.UserId.ToString()).SendAsync("ReceiveNotification",
                new
                {
                    Message = "Your appointment request has been received!",
                    AppointmentId = appointment.AppointmentId,
                    Status = "Pending"
                });

            return new AppointmentResponseDTO
            {
                Message = $"Appointment booked successfully!",
                LineNumber = randomLineNumber
            };
        }

        public async Task<IEnumerable<ClinicDTO>> GetAllClinicsAsync()
        {
            return await _context.Clinics
                .Select(c => new ClinicDTO
                {
                    ClinicId = c.ClinicId,
                    ClinicName = c.ClinicName,
                    Location = c.Location,
                    PatientCapability = c.PatientCapability
                })
                .ToListAsync();
        }

        public async Task<ClinicDTO> GetClinicByIdAsync(int clinicId)
        {
            var clinic = await _context.Clinics.FindAsync(clinicId);
            if (clinic == null) return null;

            return new ClinicDTO
            {
                ClinicId = clinic.ClinicId,
                ClinicName = clinic.ClinicName,
                Location = clinic.Location,
                PatientCapability = clinic.PatientCapability
            };
        }

        public async Task<IEnumerable<DoctorDTO>> GetAllDoctorsAsync()
        {
            return await _context.DoctorDetails
                .Include(d => d.User) // Assuming User table holds name details
                .Select(d => new DoctorDTO
                {
                    DoctorDetailsId = d.DoctorDetailsId,
                    FullName = d.User.FullName,
                    Specialization = d.Specialization
                })
                .ToListAsync();
        }

        public async Task<DoctorDTO> GetDoctorByIdAsync(int doctorId)
        {
            var doctor = await _context.DoctorDetails
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.DoctorDetailsId == doctorId);

            if (doctor == null) return null;

            return new DoctorDTO
            {
                DoctorDetailsId = doctor.DoctorDetailsId,
                FullName = doctor.User.FullName,
                Specialization = doctor.Specialization
            };
        }
    }
}
