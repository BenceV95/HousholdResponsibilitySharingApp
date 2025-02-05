using HouseholdResponsibilityAppServer.Models;
using HouseholdResponsibilityAppServer.Models.Users;


namespace HouseholdResponsibilityAppServer.Services.UserService
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<UserResponseDto> GetUserByIdAsync(int id);
        Task CreateUserAsync(UserDto userDto);
        Task UpdateUserAsync(int id, UserDto userDto);
        Task DeleteUserAsync(int id);
    }
}
