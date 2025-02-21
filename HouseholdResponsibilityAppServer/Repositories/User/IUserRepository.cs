using HouseholdResponsibilityAppServer.Models.Users;

namespace HouseholdResponsibilityAppServer.Repositories.UserRepo
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(string id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(string id);
    }

}
