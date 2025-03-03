using HouseholdResponsibilityAppServer.Models.Groups;
using HouseholdResponsibilityAppServer.Models.Households;
using HouseholdResponsibilityAppServer.Models.Users;
using HouseholdResponsibilityAppServer.Repositories.Groups;
using HouseholdResponsibilityAppServer.Repositories.HouseholdRepo;
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
            var household1 = new Household { HouseholdId = 1, CreatedAt = DateTime.Now, CreatedByUser = user1};
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
            Assert.Equal("TestGroup1",test.First().Name);
        }
    }
}
