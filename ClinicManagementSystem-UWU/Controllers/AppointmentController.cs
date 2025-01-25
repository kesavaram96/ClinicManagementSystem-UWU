using ClinicManagementSystem_UWU.Interfaces;
using ClinicManagementSystem_UWU.Models.DTO;
using ClinicManagementSystem_UWU.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClinicManagementSystem_UWU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        [HttpGet("getAll")]
        //[Authorize]
        public async Task<ActionResult<List<AppointmentResponsesDTO>>> GetAllAppointments()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            return Ok(appointments);
        }

        // Get appointment by ID
        [HttpGet("{appointmentId}")]
        public async Task<ActionResult<AppointmentResponsesDTO>> GetAppointmentById(int appointmentId)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(appointmentId);

            if (appointment == null)
            {
                return NotFound("Appointment not found");
            }

            return Ok(appointment);
        }

        // Update appointment
        [HttpPut("update/{appointmentId}")]
        public async Task<ActionResult<AppointmentResponseDTO>> UpdateAppointment(int appointmentId, [FromBody] AppointmentsDTO appointmentDto)
        {
            AppointmentResponseDTO response = await _appointmentService.UpdateAppointmentAsync(appointmentId, appointmentDto);

            if (response.Message == "Appointment not found!")
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }

        // Delete appointment
        [HttpDelete("delete/{appointmentId}")]
        public async Task<ActionResult<string>> DeleteAppointment(int appointmentId)
        {
            var response = await _appointmentService.DeleteAppointmentAsync(appointmentId);

            if (response == "Appointment not found!")
            {
                return NotFound(response);
            }

            return Ok(response);
        }
        [HttpPost("book")]
        public async Task<IActionResult> BookAppointment([FromBody] AppointmentDTO appointmentDto)
        {
            if (appointmentDto == null)
            {
                return BadRequest("Appointment data is required.");
            }

            var result = await _appointmentService.BookAppointmentAsync(appointmentDto);

            if (result.Message == "Appointment booked successfully!")
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("clinics")]
        public async Task<IActionResult> GetAllClinics()
        {
            var clinics = await _appointmentService.GetAllClinicsAsync();
            return Ok(clinics);
        }

        [HttpGet("clinics/{id}")]
        public async Task<IActionResult> GetClinicById(int id)
        {
            var clinic = await _appointmentService.GetClinicByIdAsync(id);
            if (clinic == null) return NotFound("Clinic not found.");
            return Ok(clinic);
        }

        [HttpGet("doctors")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _appointmentService.GetAllDoctorsAsync();
            return Ok(doctors);
        }

        [HttpGet("doctors/{id}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var doctor = await _appointmentService.GetDoctorByIdAsync(id);
            if (doctor == null) return NotFound("Doctor not found.");
            return Ok(doctor);
        }
    }
    }
