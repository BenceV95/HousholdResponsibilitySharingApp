using HouseholdResponsibilityAppServer.Context;
using HouseholdResponsibilityAppServer.Contracts;
using HouseholdResponsibilityAppServer.Models.Users;
using HouseholdResponsibilityAppServer.Services.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;

namespace HouseholdResponsibilityAppServerTests.UnitTests.Services
{
    public class AuthServiceUnitTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager;
        private readonly Mock<HouseholdResponsibilityAppContext> _mockContext;
        private AuthService _authService;

        public AuthServiceUnitTests()
        {
            _mockUserManager = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _mockTokenService = new Mock<ITokenService>();
            _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
            _mockContext = new Mock<HouseholdResponsibilityAppContext>(new DbContextOptions<HouseholdResponsibilityAppContext>());

            _authService = new AuthService(_mockUserManager.Object, _mockTokenService.Object, _mockRoleManager.Object, _mockContext.Object);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnSuccess_WhenUserIsCreated()
        {
            // Arrange
            var request = new RegistrationRequest("First", "Last", "test@example.com", "TestUser", "pass123");
            var role = "User";
            var user = new User { UserName = request.Username, Email = request.Email };
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _mockRoleManager.Setup(x => x.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var result = await _authService.RegisterAsync(request, role);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(request.Email, result.Email);
            Assert.Equal(request.Username, result.UserName);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnFailure_WhenUserCreationFails()
        {
            // Arrange
            var request = new RegistrationRequest("First", "Last", "test@example.com", "TestUser", "pass123");
            var role = "User";
            var user = new User { UserName = request.Username, Email = request.Email };
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "Error", Description = "User creation failed" }));

            // Act
            var result = await _authService.RegisterAsync(request, role);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Error", result.ErrorMessages.Keys);
        }

        /*
        [Fact]
        public async Task LoginAsync_ShouldReturnSuccess_WhenCredentialsAreValid()
        {
            // Arrange
            var email = "test@example.com";
            var password = "Password123!";
            var user = new User { UserName = "testuser", Email = email, Household = new Household { HouseholdId = 1, CreatedByUser = new User()} };

            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);
            _mockTokenService.Setup(x => x.CreateToken(It.IsAny<User>())).ReturnsAsync("token");

            var mockContext = new Mock<HouseholdResponsibilityAppContext>();
            mockContext.Setup(x => x.Entry(It.IsAny<User>()).Reference(It.IsAny<Expression<Func<User, object>>>()).Load()).Verifiable();
            _authService = new AuthService(_mockUserManager.Object, _mockTokenService.Object, _mockRoleManager.Object, mockContext.Object);

            // Act
            var result = await _authService.LoginAsync(email, password);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(email, result.Email);
            Assert.Equal(user.UserName, result.UserName);
            Assert.Equal("token", result.Token);
            mockContext.Verify();
        }
        */

        [Fact]
        public async Task LoginAsync_ShouldReturnFailure_WhenEmailIsInvalid()
        {
            // Arrange
            var email = "test@example.com";
            var password = "Password123!";
            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);

            // Act
            var result = await _authService.LoginAsync(email, password);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Bad credentials", result.ErrorMessages.Keys);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnFailure_WhenPasswordIsInvalid()
        {
            // Arrange
            var email = "test@example.com";
            var password = "Password123!";
            var user = new User { UserName = "testuser", Email = email };
            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(false);

            // Act
            var result = await _authService.LoginAsync(email, password);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Bad credentials", result.ErrorMessages.Keys);
        }

        [Fact]
        public void GetClaimsFromHttpContext_ShouldReturnUserClaims_WhenHttpContextIsValid()
        {
            // Arrange
            var userId = "123";
            var userName = "testuser";
            var email = "test@example.com";
            var householdId = "1";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email, email),
                new Claim("householdId", householdId),
                new Claim(ClaimTypes.Role, "User")
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var principal = new ClaimsPrincipal(identity);
            var context = new DefaultHttpContext { User = principal };

            // Act
            var result = _authService.GetClaimsFromHttpContext(context);

            // Assert
            Assert.Equal(userId, result.UserId);
            Assert.Equal(userName, result.UserName);
            Assert.Equal(email, result.Email);
            Assert.Equal(householdId, result.HouseholdId);
            Assert.Contains("User", result.Roles);
        }
    }
}
