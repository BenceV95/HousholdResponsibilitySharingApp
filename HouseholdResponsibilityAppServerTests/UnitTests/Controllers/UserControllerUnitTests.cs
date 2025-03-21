using HouseholdResponsibilityAppServer.Models.Users;
using HouseholdResponsibilityAppServer.Services.Authentication;
using HouseholdResponsibilityAppServer.Services.Invitation;
using HouseholdResponsibilityAppServer.Services.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HouseholdResponsibilityAppServerTests.UnitTests.Controllers
{
    public class UserControllerUnitTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IInvitationService> _mockInvitationService;
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<ILogger<GroupController>> _mockLogger;
        private readonly UserController _controller;

        public UserControllerUnitTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockInvitationService = new Mock<IInvitationService>();
            _mockAuthService = new Mock<IAuthService>();
            _mockLogger = new Mock<ILogger<GroupController>>();
            _controller = new UserController(_mockUserService.Object, _mockInvitationService.Object, _mockAuthService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllUser_ReturnsOkResult_WithListOfUsers()
        {
            // Arrange
            var users = new List<UserResponseDto> { new UserResponseDto { UserResponseDtoId = "1", Username = "TestUser", Email = "test@example.com", FirstName = "Test", LastName = "User", Role = "User", CreatedAt = DateTime.Now, HouseholdId = 1 } };
            _mockUserService.Setup(service => service.GetAllUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUser();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<UserResponseDto>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetUserById_ReturnsOkResult_WithUser()
        {
            // Arrange
            var user = new UserResponseDto { UserResponseDtoId = "1", Username = "TestUser", Email = "test@example.com", FirstName = "Test", LastName = "User", Role = "User", CreatedAt = DateTime.Now, HouseholdId = 1 };
            _mockUserService.Setup(service => service.GetUserByIdAsync("1")).ReturnsAsync(user);

            // Act
            var result = await _controller.GetUserById("1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<UserResponseDto>(okResult.Value);
            Assert.Equal("1", returnValue.UserResponseDtoId);
        }

        [Fact]
        public async Task CreateUser_ReturnsOkResult_WithSuccessMessage()
        {
            // Arrange
            var userDto = new UserDto { Username = "NewUser", Email = "newuser@example.com", FirstName = "New", LastName = "User", Password = "password" };

            // Act
            var result = await _controller.CreateUser(userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("User created successfully!", okResult.Value.ToString());
        }

        [Fact]
        public async Task UpdateUser_ReturnsOkResult_WithSuccessMessage()
        {
            // Arrange
            var userDto = new UserDto { Username = "UpdatedUser", Email = "updateduser@example.com", FirstName = "Updated", LastName = "User", Password = "password" };

            // Act
            var result = await _controller.UpdateUser("1", userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Update successful!", okResult.Value.ToString());
        }

        [Fact]
        public async Task DeleteUser_ReturnsNoContentResult()
        {
            // Arrange
            _mockUserService.Setup(service => service.DeleteUserAsync("1")).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteUser("1");

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteSelf_ReturnsNoContentResult()
        {
            // Arrange
            var userClaims = new UserClaims { UserId = "1", UserName = "TestUser", Email = "test@example.com", HouseholdId = "1", Roles = new List<string> { "User" } };
            _mockAuthService.Setup(service => service.GetClaimsFromHttpContext(It.IsAny<HttpContext>())).Returns(userClaims);
            _mockUserService.Setup(service => service.DeleteUserAsync("1")).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteSelf();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetUsersByHouseholdId_ReturnsOkResult_WithListOfUsers()
        {
            // Arrange
            var userClaims = new UserClaims { UserId = "1", UserName = "TestUser", Email = "test@example.com", HouseholdId = "1", Roles = new List<string> { "User" } };
            var users = new List<UserResponseDto> { new UserResponseDto { UserResponseDtoId = "1", Username = "TestUser", Email = "test@example.com", FirstName = "Test", LastName = "User", Role = "User", CreatedAt = DateTime.Now, HouseholdId = 1 } };
            _mockAuthService.Setup(service => service.GetClaimsFromHttpContext(It.IsAny<HttpContext>())).Returns(userClaims);
            _mockUserService.Setup(service => service.GetAllUsersByHouseholdIdAsync(userClaims)).ReturnsAsync(users);

            // Act
            var result = await _controller.GetUsersByHouseholdId();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<UserResponseDto>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task LeaveHousehold_ReturnsNoContentResult()
        {
            // Arrange
            var userClaims = new UserClaims { UserId = "1", UserName = "TestUser", Email = "test@example.com", HouseholdId = "1", Roles = new List<string> { "User" } };
            _mockAuthService.Setup(service => service.GetClaimsFromHttpContext(It.IsAny<HttpContext>())).Returns(userClaims);
            _mockUserService.Setup(service => service.LeaveHouseholdAsync("1")).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.LeaveHousehold();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}

