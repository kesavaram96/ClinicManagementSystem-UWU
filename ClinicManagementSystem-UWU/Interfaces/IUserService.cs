using ClinicManagementSystem_UWU.Models.Auth;
using ClinicManagementSystem_UWU.Models.DTO;

namespace ClinicManagementSystem_UWU.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(UserCreationDto userDto);
        //Task AddUserAsync(User user);
        Task AssignRoleAsync(int userId, int roleId);
        Task<List<Role>> GetUserRolesAsync(int userId);
    }

}
