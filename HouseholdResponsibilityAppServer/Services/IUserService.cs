using HouseholdResponsibilityAppServer.DTOs;
using HouseholdResponsibilityAppServer.Models;


namespace HouseholdResponsibilityAppServer.Services
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
