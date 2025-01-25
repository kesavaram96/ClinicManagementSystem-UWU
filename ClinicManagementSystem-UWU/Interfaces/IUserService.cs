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
        Task<string> LoginAsync(LoginDto loginDto);
        Task<bool> CreateUserAsync(CreateUserDto createUserDto);
        Task<List<User>> GetUsersList();
        Task<UserEditDTO> GetUser(int userId);
        Task<string> EditUser(int userId, EditUserDTO userDto);
        Task<string> DeleteUser(int userId);

        Task<PatientDetailsDTO> GetPatient(int userId);
    }

}
