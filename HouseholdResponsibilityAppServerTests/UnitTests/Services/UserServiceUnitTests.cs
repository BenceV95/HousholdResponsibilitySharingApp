using HouseholdResponsibilityAppServer.Models.Households;
using HouseholdResponsibilityAppServer.Models.Users;
using HouseholdResponsibilityAppServer.Repositories.UserRepo;
using HouseholdResponsibilityAppServer.Services.Authentication;
using HouseholdResponsibilityAppServer.Services.UserService;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HouseholdResponsibilityAppServerTests.UnitTests.Services
{
    public class UserServiceUnitTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _userService;

        public UserServiceUnitTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "1", UserName = "user1", Email = "user1@example.com", FirstName = "First1", LastName = "Last1", CreatedAt = DateTime.UtcNow },
                new User { Id = "2", UserName = "user2", Email = "user2@example.com", FirstName = "First2", LastName = "Last2", CreatedAt = DateTime.UtcNow }
            };
            _userRepositoryMock.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("user1", result.First().Username);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsUser()
        {
            // Arrange
            var user = new User { Id = "1", UserName = "user1", Email = "user1@example.com", FirstName = "First1", LastName = "Last1", CreatedAt = DateTime.UtcNow };
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync("1")).ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserByIdAsync("1");

            // Assert
            Assert.Equal("user1", result.Username);
        }

        [Fact]
        public async Task CreateUserAsync_CreatesUser()
        {
            // Arrange
            var userDto = new UserDto { Username = "user1", Email = "user1@example.com", FirstName = "First1", LastName = "Last1", Password = "password" };

            // Act
            await _userService.CreateUserAsync(userDto);

            // Assert
            _userRepositoryMock.Verify(repo => repo.AddUserAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_UpdatesUser()
        {
            // Arrange
            var user = new User { Id = "1", UserName = "user1", Email = "user1@example.com", FirstName = "First1", LastName = "Last1", CreatedAt = DateTime.UtcNow };
            var userDto = new UserDto { Username = "updatedUser", Email = "updated@example.com", FirstName = "UpdatedFirst", LastName = "UpdatedLast", Password = "newpassword" };
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync("1")).ReturnsAsync(user);

            // Act
            await _userService.UpdateUserAsync("1", userDto);

            // Assert
            _userRepositoryMock.Verify(repo => repo.UpdateUserAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_DeletesUser()
        {
            // Act
            await _userService.DeleteUserAsync("1");

            // Assert
            _userRepositoryMock.Verify(repo => repo.DeleteUserAsync("1"), Times.Once);
        }

        [Fact]
        public async Task GetAllUsersByHouseholdIdAsync_ReturnsUsersByHouseholdId()
        {
            // Arrange
            var household = new Household
            {
                HouseholdId = 1,
                CreatedByUser = new User { Id = "1" }
            };
            var household2 = new Household
            {
                HouseholdId = 2,
                CreatedByUser = new User { Id = "2" }
            };
            var userClaims = new UserClaims { HouseholdId = "1" };
            var users = new List<User>
            {
                new User 
                {
                    Id = "1",
                    UserName = "user1",
                    Email = "user1@example.com",
                    FirstName = "First1",
                    LastName = "Last1",
                    CreatedAt = DateTime.UtcNow,
                    Household = household
                },
                new User
                {
                    Id = "2",
                    UserName = "user2",
                    Email = "user2@example.com",
                    FirstName = "First2",
                    LastName = "Last2",
                    CreatedAt = DateTime.UtcNow,
                    Household = household2
                }
            };
            _userRepositoryMock.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllUsersByHouseholdIdAsync(userClaims);

            // Assert
            Assert.Single(result);
            Assert.Equal("user1", result.First().Username);
        }
    }
}
