using HouseholdResponsibilityAppServer.Models.Groups;
using HouseholdResponsibilityAppServer.Models.Households;
using HouseholdResponsibilityAppServer.Models.Users;
using HouseholdResponsibilityAppServer.Repositories.Groups;
using HouseholdResponsibilityAppServer.Repositories.HouseholdRepo;
using HouseholdResponsibilityAppServer.Services.Authentication;
using HouseholdResponsibilityAppServer.Services.Groups;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;

namespace HouseholdResponsibilityAppServerTests.UnitTests.Services
{
    public class GroupServiceUnitTests
    {
        private readonly Mock<IGroupRepository> _groupRepositoryMock;
        private readonly Mock<IHouseholdRepository> _householdRepositoryMock;
        private readonly GroupService _groupService;

        public GroupServiceUnitTests()
        {
            _groupRepositoryMock = new Mock<IGroupRepository>();
            _householdRepositoryMock = new Mock<IHouseholdRepository>();
            _groupService = new GroupService(_groupRepositoryMock.Object, _householdRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllGroupsAsync_ReturnsAllGroups()
        {
            // Arrange
            var user1 = new User { UserName = "TestUser1" };
            var user2 = new User { UserName = "TestUser2" };
            var household1 = new Household { HouseholdId = 1, CreatedAt = DateTime.Now, CreatedByUser = user1 };
            var household2 = new Household { HouseholdId = 2, CreatedAt = DateTime.Now, CreatedByUser = user2 };

            var testTaskGroup = new List<TaskGroup>
            {
                new() { GroupId = 1, Household = household1, Name = "TestGroup1" },
                new() { GroupId = 2, Household = household2, Name = "TestGroup2" }
            };

            _groupRepositoryMock.Setup(repo => repo.GetAllGroupsAsync()).ReturnsAsync(testTaskGroup);

            // Act
            var test = await _groupService.GetAllGroupsAsync();

            // Assert
            Assert.Equal(2, test.Count());
            Assert.Equal("TestGroup1", test.First().Name);
        }

        [Fact]
        public async Task GetGroupByIdAsync_ReturnsGroup()
        {
            // Arrange
            var user1 = new User { UserName = "TestUser1" };
            var household1 = new Household { HouseholdId = 1, CreatedAt = DateTime.Now, CreatedByUser = user1 };
            var group = new TaskGroup { GroupId = 1, Household = household1, Name = "TestGroup1" };

            _groupRepositoryMock.Setup(repo => repo.GetGroupByIdAsync(1)).ReturnsAsync(group);

            // Act
            var test = _groupService.GetGroupByIdAsync(1);

            // Assert
            Assert.Equal("TestGroup1", test.Result.Name);
        }

        [Fact]
        public async Task CreateGroupAsync_CreatesGroup()
        {
            // Arrange
            var postGroupDto = new PostGroupDto { GroupName = "NewGroup" };
            var userClaims = new UserClaims { HouseholdId = "1" };
            var user1 = new User { UserName = "TestUser1" };
            var household1 = new Household { HouseholdId = 1, CreatedAt = DateTime.Now, CreatedByUser = user1 };

            _householdRepositoryMock.Setup(repo => repo.GetHouseholdByIdAsync(1)).ReturnsAsync(household1);
            //_groupRepositoryMock.Setup(repo => repo.AddGroupAsync(It.IsAny<TaskGroup>())).Returns(Task.CompletedTask);

            // Act
            await _groupService.CreateGroupAsync(postGroupDto, userClaims);

            // Assert
            _groupRepositoryMock.Verify(
                repo => repo.AddGroupAsync(It.Is<TaskGroup>(g => g.Name == "NewGroup" && g.Household == household1)),
                Times.Once);
        }

        [Fact]
        public async Task CreateGroupAsync_ThrowsArgumentException_WhenHouseholdIdIsNotANumber()
        {
            // Arrange
            var postGroupDto = new PostGroupDto { GroupName = "Test Group" };
            var userClaims = new UserClaims { HouseholdId = "NotANumber" };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _groupService.CreateGroupAsync(postGroupDto, userClaims));
            Assert.Equal("Cannot create group, user is not in a household!", exception.Message);
        }

        [Fact]
        public async Task UpdateGroupAsync_UpdatesGroup()
        {
            // Arrange
            var group = new TaskGroup { GroupId = 1, Name = "OldName" };
            var groupDto = new GroupDto { Name = "NewName" };
            _groupRepositoryMock.Setup(repo => repo.GetGroupByIdAsync(1)).ReturnsAsync(group);

            // Act
            await _groupService.UpdateGroupAsync(1, groupDto);

            // Assert
            _groupRepositoryMock.Verify(repo => repo.UpdateGroupAsync(It.Is<TaskGroup>(g => g.Name == "NewName")), Times.Once);
        }

        [Fact]
        public async Task UpdateGroupAsync_WithIncorrectName_ThrowException()
        {
            // Arrange
            var group = new TaskGroup { GroupId = 1, Name = "OldName" };
            var groupDto = new GroupDto { Name = " " };
            var groupDto1 = new GroupDto { Name = "" };

            _groupRepositoryMock.Setup(repo => repo.GetGroupByIdAsync(1)).ReturnsAsync(group);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _groupService.UpdateGroupAsync(1, groupDto));
            var exception1 = await Assert.ThrowsAsync<ArgumentException>(() => _groupService.UpdateGroupAsync(1, groupDto1));
        }

        [Fact]
        public async Task DeleteGroupAsync_DeletesGroup()
        {
            // Act
            await _groupService.DeleteGroupAsync(1);

            // Assert
            _groupRepositoryMock.Verify(repo => repo.DeleteGroupAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetGroupsByHouseholdIdAsync_ReturnsGroups()
        {
            // Arrange
            var userClaims = new UserClaims { HouseholdId = "1" };
            var user1 = new User { UserName = "TestUser1" };
            var household1 = new Household { HouseholdId = 1, CreatedAt = DateTime.Now, CreatedByUser = user1 };
            var testTaskGroups = new List<TaskGroup>
            {
                new() { GroupId = 1, Household = household1, Name = "TestGroup1" },
                new() { GroupId = 2, Household = household1, Name = "TestGroup2" },
                new() { GroupId = 3, Household = household1, Name = "TestGroup3" }
            };

            _groupRepositoryMock.Setup(repo => repo.GetGroupsByHouseholdId(1)).ReturnsAsync(testTaskGroups);

            // Act
            var test = await _groupService.GetGroupsByHouseholdIdAsync(userClaims);

            // Assert
            Assert.Equal(3, test.Count());
            Assert.Equal("TestGroup1", test.First().Name);
        }
    }
}
