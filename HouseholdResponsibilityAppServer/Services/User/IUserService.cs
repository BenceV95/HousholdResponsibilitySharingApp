using HouseholdResponsibilityAppServer.Models.Users;
using HouseholdResponsibilityAppServer.Services.Authentication;


namespace HouseholdResponsibilityAppServer.Services.UserService
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<UserResponseDto> GetUserByIdAsync(string id);
        Task CreateUserAsync(UserDto userDto);
        Task UpdateUserAsync(string id, UserDto userDto);
        Task DeleteUserAsync(string id);
        Task<IEnumerable<UserResponseDto>> GetAllUsersByHouseholdIdAsync(UserClaims userClaims);
        Task LeaveHouseholdAsync(string userId);
    }
}
