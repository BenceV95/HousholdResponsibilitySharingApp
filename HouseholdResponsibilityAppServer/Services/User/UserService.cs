using HouseholdResponsibilityAppServer.Models.Users;
using HouseholdResponsibilityAppServer.Repositories.UserRepo;
using Microsoft.AspNetCore.Identity;

namespace HouseholdResponsibilityAppServer.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();

            return users.Select(user => new UserResponseDto
            {
                UserResponseDtoId = user.Id,
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                //IsAdmin = user.IsAdmin, roletabléből majd
                CreatedAt = user.CreatedAt,
            }).ToList();

        }

        public async Task<UserResponseDto> GetUserByIdAsync(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            return new UserResponseDto
            {
                UserResponseDtoId = user.Id,
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsAdmin = user.IsAdmin,
                CreatedAt = user.CreatedAt,
            };
        }

        public async Task CreateUserAsync(UserDto userDto)
        {
            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                PasswordHash = _passwordHasher.HashPassword(null, userDto.Password),
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddUserAsync(user);
        }

        public async Task UpdateUserAsync(string userId, UserDto userDto)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            user.Username = userDto.Username;
            user.Email = userDto.Email;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.PasswordHash = _passwordHasher.HashPassword(null, userDto.Password);

            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(string userId)
        {
            await _userRepository.DeleteUserAsync(userId);
        }

    }
}
