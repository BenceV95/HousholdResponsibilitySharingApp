using HouseholdResponsibilityAppServer.Models;
using HouseholdResponsibilityAppServer.Models.Users;


namespace HouseholdResponsibilityAppServer.Services.UserService
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<UserResponseDto> GetUserByIdAsync(string id);
        Task CreateUserAsync(UserDto userDto);
        Task UpdateUserAsync(string id, UserDto userDto);
        Task DeleteUserAsync(string id);
    }
}
