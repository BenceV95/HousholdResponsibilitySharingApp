using HouseholdResponsibilityAppServer.Models.Households;
using HouseholdResponsibilityAppServer.Models.Users;
using HouseholdResponsibilityAppServer.Repositories.HouseholdRepo;
using HouseholdResponsibilityAppServer.Repositories.UserRepo;
using HouseholdResponsibilityAppServer.Services.Authentication;
using HouseholdResponsibilityAppServer.Services.HouseholdService;
using Moq;

namespace HouseholdResponsibilityAppServerTests.UnitTests.Services
{
    public class HouseholdServiceUnitTests
    {
        private readonly Mock<IHouseholdRepository> _mockHouseholdRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly HouseholdService _householdService;

        public HouseholdServiceUnitTests()
        {
            _mockHouseholdRepository = new Mock<IHouseholdRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _householdService = new HouseholdService(_mockHouseholdRepository.Object, _mockUserRepository.Object);
        }

        [Fact]
        public async Task GetAllHouseholdsAsync_ReturnsHouseholds()
        {
            // Arrange
            var households = new List<Household>
            {
                new Household { HouseholdId = 1, Name = "Household1", CreatedAt = DateTime.UtcNow, CreatedByUser = new User { UserName = "User1" } },
                new Household { HouseholdId = 2, Name = "Household2", CreatedAt = DateTime.UtcNow, CreatedByUser = new User { UserName = "User2" } }
            };
            _mockHouseholdRepository.Setup(repo => repo.GetAllHouseholdsAsync()).ReturnsAsync(households);

            // Act
            var result = await _householdService.GetAllHouseholdsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Household1", result.First().Name);
        }

        [Fact]
        public async Task GetHouseholdByIdAsync_ReturnsHousehold()
        {
            // Arrange
            var household = new Household
            {
                HouseholdId = 1,
                Name = "Household1",
                CreatedAt = DateTime.UtcNow,
                CreatedByUser = new User { UserName = "User1" }
            };
            _mockHouseholdRepository.Setup(repo => repo.GetHouseholdByIdAsync(1)).ReturnsAsync(household);

            // Act
            var result = await _householdService.GetHouseholdByIdAsync(1);

            // Assert
            Assert.Equal(household.Name, result.Name);
        }

        [Fact]
        public async Task CreateHouseholdAsync_CreatesHousehold()
        {
            // Arrange
            var userClaims = new UserClaims { UserId = "1", UserName = "User1" };
            var user = new User { Id = "1", UserName = "User1" };
            var householdDto = new HouseholdDto { HouseholdName = "NewHousehold" };
            var household = new Household { HouseholdId = 1, Name = "NewHousehold", CreatedByUser = user, CreatedAt = DateTime.UtcNow };

            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync("1")).ReturnsAsync(user);
            _mockHouseholdRepository.Setup(repo => repo.AddHouseholdAsync(It.IsAny<Household>())).ReturnsAsync(household);

            // Act
            var result = await _householdService.CreateHouseholdAsync(householdDto, userClaims);

            // Assert
            Assert.Equal("NewHousehold", result.Name);
            Assert.Equal(household.HouseholdId, result.HouseholdId);
            _mockUserRepository.Verify(repo => repo.UpdateUserAsync(user), Times.Once);
        }

        [Fact]
        public async Task CreateHouseholdAsync_Throws_WhenUserIsAlreadyInAHousehold()
        {
            // Arrange
            var userClaims = new UserClaims { UserId = "1", UserName = "User1" };

            // chaos lol
            var user = new User
            {
                Id = "1",
                UserName = "User1",
                Household = new Household
                {
                    HouseholdId = 1,
                    Name = "NewHousehold",
                    CreatedByUser = new User {
                        Id = "1",
                        UserName = "User1",
                    },
                    CreatedAt = DateTime.UtcNow
                }
            };
            var householdDto = new HouseholdDto { HouseholdName = "NewHousehold" };

            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync("1")).ReturnsAsync(user);

            // Act

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _householdService.CreateHouseholdAsync(householdDto, userClaims));
        }

        [Fact]
        public async Task JoinHousehold_AddsUserToHousehold()
        {
            // Arrange
            var user = new User { Id = "1", UserName = "User1" };
            var householdAdminUser = new User() { Id = "2", UserName = "admin" };
            var household = new Household
            {
                HouseholdId = 1,
                Name = "Household1",
                CreatedByUser = user,
                Users = new List<User>() {householdAdminUser}
            };

            _mockHouseholdRepository.Setup(repo => repo.GetHouseholdByIdAsync(1)).ReturnsAsync(household);
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync("1")).ReturnsAsync(user);

            // Act
            await _householdService.JoinHousehold(1, "1");

            // Assert
            Assert.Contains(user, household.Users);
            _mockUserRepository.Verify(repo => repo.UpdateUserAsync(user), Times.Once);
            _mockHouseholdRepository.Verify(repo => repo.UpdateHouseholdAsync(household), Times.Once);
        }

        [Fact]
        public async Task JoinHousehold_ThrowsException_WhenUserIsAlreadyMember()
        {
            // Arrange
            var user = new User { Id = "1", UserName = "User1" };
            var householdAdminUser = new User() { Id = "2", UserName = "admin" };
            var household = new Household
            {
                HouseholdId = 1,
                Name = "Household1",
                Users = new List<User> { user },
                CreatedByUser = householdAdminUser
            };

            _mockHouseholdRepository.Setup(repo => repo.GetHouseholdByIdAsync(1)).ReturnsAsync(household);
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync("1")).ReturnsAsync(user);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _householdService.JoinHousehold(1, "1"));
        }

        [Fact]
        public async Task JoinHousehold_ThrowsException_WhenUserOrHouseholdNotFound()
        {
            // Arrange
            _mockHouseholdRepository.Setup(repo => repo.GetHouseholdByIdAsync(It.IsAny<int>())).ReturnsAsync((Household)null);
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(It.IsAny<string>())).ReturnsAsync((User)null);

            // Act & Assert
            var test = await Assert.ThrowsAsync<KeyNotFoundException>(() => _householdService.JoinHousehold(1, "1"));
            Assert.Equal("User or household not found.",test.Message);
        }

        [Fact]
        public async Task UpdateHouseholdAsync_UpdatesHousehold()
        {
            // Arrange
            var user = new User { Id = "1", UserName = "User1" };
            var household = new Household { HouseholdId = 1, Name = "OldName", CreatedByUser = user};
            var householdDto = new HouseholdDto { HouseholdName = "NewName" };

            _mockHouseholdRepository.Setup(repo => repo.GetHouseholdByIdAsync(1)).ReturnsAsync(household);

            // Act
            await _householdService.UpdateHouseholdAsync(1, householdDto);

            // Assert
            Assert.Equal(householdDto.HouseholdName, household.Name);
            _mockHouseholdRepository.Verify(repo => repo.UpdateHouseholdAsync(household), Times.Once);
        }

        [Fact]
        public async Task DeleteHouseholdAsync_DeletesHousehold()
        {
            // Arrange
            _mockHouseholdRepository.Setup(repo => repo.DeleteHouseholdAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _householdService.DeleteHouseholdAsync(1);

            // Assert
            _mockHouseholdRepository.Verify(repo => repo.DeleteHouseholdAsync(1), Times.Once);
        }
    }
}
