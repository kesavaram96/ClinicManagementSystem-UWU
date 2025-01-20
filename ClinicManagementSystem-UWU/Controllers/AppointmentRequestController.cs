using ClinicManagementSystem_UWU.Interfaces;
using ClinicManagementSystem_UWU.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem_UWU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentRequestController : ControllerBase
    {
        private readonly IAppointmentRequestService _service;

        public AppointmentRequestController(IAppointmentRequestService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] string? status)
        {
            var result = await _service.GetAllAsync(status);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentRequestDto request)
        {
            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
