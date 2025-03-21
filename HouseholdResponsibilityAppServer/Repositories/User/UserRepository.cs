using HouseholdResponsibilityAppServer.Context;
using HouseholdResponsibilityAppServer.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace HouseholdResponsibilityAppServer.Repositories.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly HouseholdResponsibilityAppContext _context;

        public UserRepository(HouseholdResponsibilityAppContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
                return await _context.Users
                    .Include(u => u.Household)
                    .ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            var user = await _context.Users
                .Include(u => u.Household)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user ?? throw new KeyNotFoundException($"User with ID {userId} not found.");
        }

        public async Task AddUserAsync(User user)
        {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);

            int affectedRows = await _context.SaveChangesAsync();

            if (affectedRows == 0) throw new KeyNotFoundException($"User with ID {user.Id} not found.");
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
