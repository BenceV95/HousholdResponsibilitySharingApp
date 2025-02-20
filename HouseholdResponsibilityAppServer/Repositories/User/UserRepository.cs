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
            try
            {
                return await _context.Users
                    .Include(u => u.Household) // Betölti a kapcsolódó Household entitást
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Database error: Unable to fetch users.");
            }
        }


        public async Task<User> GetUserByIdAsync(string userId)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

                return user ?? throw new KeyNotFoundException($"User with ID {userId} not found.");
            }
            catch (Exception ex)
            {
                throw new Exception("Database error: Unable to fetch user by ID.");
            }
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Database error: Unable to create user.");
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);

            int affectedRows = await _context.SaveChangesAsync();

            if (affectedRows == 0) throw new KeyNotFoundException($"User with ID {user.Id} not found.");
        }

        /*public async Task DeleteUserAsync(int userId)
        {
            var user = new User { UserId = userId };

            _context.Entry(user).State = EntityState.Deleted;

            int affectedRows = await _context.SaveChangesAsync();

            if (affectedRows == 0) throw new KeyNotFoundException($"User with ID {userId} not found.");

        }
        */

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
