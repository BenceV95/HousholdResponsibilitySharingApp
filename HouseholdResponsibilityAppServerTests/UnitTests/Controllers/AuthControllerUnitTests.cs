using HouseholdResponsibilityAppServer.Controllers;
using HouseholdResponsibilityAppServer.Contracts;
using HouseholdResponsibilityAppServer.Repositories.UserRepo;
using HouseholdResponsibilityAppServer.Services.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;
using HouseholdResponsibilityAppServer.Models.Users;

namespace HouseholdResponsibilityAppServerTests.UnitTests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly AuthController _authController;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockTokenService = new Mock<ITokenService>();
            _authController = new AuthController(
                _mockAuthService.Object,
                _mockUserRepository.Object,
                _mockTokenService.Object);
        }

        [Fact]
        public async Task Register_ValidRequest_ReturnsCreatedAtAction()
        {
            // Arrange
            var request = new RegistrationRequest("First", "Last", "test@example.com", "TestUser", "pass123");
            var authResult = new AuthResult(true, "test@example.com", "TestUser", null, null);
            _mockAuthService.Setup(s => s.RegisterAsync(request, "User")).ReturnsAsync(authResult);

            // Act
            var result = await _authController.Register(request);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var registrationResponse = Assert.IsType<RegistrationResponse>(actionResult.Value);
            Assert.Equal(authResult.Email, registrationResponse.Email);
            Assert.Equal(authResult.UserName, registrationResponse.UserName);
        }

        [Fact]
        public async Task Register_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var request = new RegistrationRequest("First", "Last", "test@example.com", "TestUser", "pass123");

            _authController.ModelState.AddModelError("Error", "Invalid model state");

            // Act
            var result = await _authController.Register(request);

            // Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(actionResult.Value);
        }

        [Fact]
        public async Task Login_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var request = new AuthRequest("test@example.com", "password");
            var authResult = new AuthResult(false, null, null, null, null);
            authResult.ErrorMessages.Add("Error", "Invalid credentials");
            _mockAuthService.Setup(s => s.LoginAsync(request.Email, request.Password)).ReturnsAsync(authResult);

            // Act
            var result = await _authController.Login(request);

            // Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(actionResult.Value);
        }

        /*
        [Fact]
        public async Task Login_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new AuthRequest("test@example.com", "password");
            var authResult = new AuthResult(true, "test@example.com", "TestUser", "token", 1);
            _mockAuthService.Setup(s => s.LoginAsync(request.Email, request.Password)).ReturnsAsync(authResult);

            // Act
            var result = await _authController.Login(request);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var authResponse = Assert.IsType<AuthResponse>(actionResult.Value);
            Assert.Equal(authResult.Email, authResponse.Email);
            Assert.Equal(authResult.UserName, authResponse.UserName);
            Assert.Equal(authResult.HouseholdId, authResponse.HouseholdId);
        }
        
        [Fact]
        public void Logout_ReturnsOk()
        {
            // Act
            var result = _authController.Logout();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<dynamic>(actionResult.Value);
            Assert.Equal("Logout successful", response.message);
        }
        

        [Fact]
        public async Task RefreshToken_ValidRequest_ReturnsOk()
        {
            // Arrange
            var userClaims = new UserClaims { UserId = "testUserId" };
            var user = new User { Id = "testUserId", Email = "test@example.com", UserName = "TestUser" };
            var token = "newToken";

            _mockAuthService.Setup(s => s.GetClaimsFromHttpContext(It.IsAny<HttpContext>())).Returns(userClaims);
            _mockUserRepository.Setup(r => r.GetUserByIdAsync(userClaims.UserId)).ReturnsAsync(user);
            _mockTokenService.Setup(t => t.CreateToken(user)).ReturnsAsync(token);

            // Act
            var result = await _authController.RefreshToken();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<dynamic>(actionResult.Value);
            Assert.Equal("Token refreshed successfully", response.message);
        }

        [Fact]
        public async Task RefreshToken_InvalidUser_ReturnsUnauthorized()
        {
            // Arrange
            var userClaims = new UserClaims { UserId = "invalidUserId" };

            _mockAuthService.Setup(s => s.GetClaimsFromHttpContext(It.IsAny<HttpContext>())).Returns(userClaims);
            _mockUserRepository.Setup(r => r.GetUserByIdAsync(userClaims.UserId)).ReturnsAsync((User)null);

            // Act
            var result = await _authController.RefreshToken();

            // Assert
            var actionResult = Assert.IsType<UnauthorizedResult>(result);
        }
        */
    }
}
