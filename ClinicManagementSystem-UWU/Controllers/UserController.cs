using ClinicManagementSystem_UWU.Interfaces;
using ClinicManagementSystem_UWU.Models.Auth;
using ClinicManagementSystem_UWU.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem_UWU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(UserCreationDto userDto)
        {
            var user = await _userService.CreateUserAsync(userDto);
            return Ok(new { Message = "User created successfully", UserId = user.UserId });
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole(int userId, int roleId)
        {
            await _userService.AssignRoleAsync(userId, roleId);
            return Ok("Role assigned successfully");
        }

        [HttpGet("{userId}/roles")]
        public async Task<IActionResult> GetUserRoles(int userId)
        {
            var roles = await _userService.GetUserRolesAsync(userId);
            return Ok(roles);
        }
    }
}
