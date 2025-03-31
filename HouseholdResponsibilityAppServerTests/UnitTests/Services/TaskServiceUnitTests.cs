using HouseholdResponsibilityAppServer.Models.HouseholdTasks;
using HouseholdResponsibilityAppServer.Models.Task;
using HouseholdResponsibilityAppServer.Repositories.Groups;
using HouseholdResponsibilityAppServer.Repositories.HouseholdRepo;
using HouseholdResponsibilityAppServer.Repositories.HouseholdTasks;
using HouseholdResponsibilityAppServer.Repositories.UserRepo;
using HouseholdResponsibilityAppServer.Services.HouseholdTaskServices;
using HouseholdResponsibilityAppServer.Services.UserService;
using HouseholdResponsibilityAppServer.Services.Groups;
using HouseholdResponsibilityAppServer.Services.Authentication;
using Microsoft.Extensions.Logging;
using Moq;
using HouseholdResponsibilityAppServer.Models.Groups;
using HouseholdResponsibilityAppServer.Models.Households;
using HouseholdResponsibilityAppServer.Models.Users;

namespace HouseholdResponsibilityAppServerTests.UnitTests.Services
{
    public class TaskServiceUnitTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IGroupService> _groupServiceMock;
        private readonly Mock<IHouseholdTasksRepository> _householdTaskRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IGroupRepository> _groupRepositoryMock;
        private readonly Mock<IHouseholdRepository> _householdRepositoryMock;
        private readonly Mock<ILogger<HouseholdTaskService>> _loggerMock;
        private readonly HouseholdTaskService _householdTaskService;

        public TaskServiceUnitTests()
        {
            _householdTaskRepositoryMock = new Mock<IHouseholdTasksRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _groupRepositoryMock = new Mock<IGroupRepository>();
            _householdRepositoryMock = new Mock<IHouseholdRepository>();

            _householdTaskService = new HouseholdTaskService(
                _householdTaskRepositoryMock.Object,
                _userRepositoryMock.Object,
                _groupRepositoryMock.Object,
                _householdRepositoryMock.Object
            );
        }

        [Fact]
        public async Task GetAllTasksAsync_ShouldReturnAllTasks()
        {
            // Arrange
            var tasks = new List<HouseholdTask>
            {
                new HouseholdTask { TaskId = 1, Title = "Test Task" },
                new HouseholdTask { TaskId = 2, Title = "Test Task 2" }
            };
            _householdTaskRepositoryMock.Setup(repo => repo.GetAllTasksAsync()).ReturnsAsync(tasks);

            // Act
            var result = await _householdTaskService.GetallTasksAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Test Task", result.First().Title);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTaskById()
        {
            // Arrange
            var task = new HouseholdTask { TaskId = 1, Title = "Test Task" };
            _householdTaskRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(task);

            // Act
            var result = await _householdTaskService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(task.Title, result.Title);
        }

        [Fact]
        public async Task GetAllTasksByHouseholdIdAsync_ShouldReturnTasksByHouseholdId()
        {
            // Arrange
            var userClaims = new UserClaims { HouseholdId = "1" };
            var tasks = new List<HouseholdTask>
            {
                new HouseholdTask { TaskId = 1, Title = "Test Task" },
                new HouseholdTask { TaskId = 2, Title = "Test Task 2" }
            };
            _householdTaskRepositoryMock.Setup(repo => repo.GetAllTasksByHouseholdIdAsync(1)).ReturnsAsync(tasks);

            // Act
            var result = await _householdTaskService.GetallTasksByHouseholdIdAsync(userClaims);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Test Task", result.First().Title);
        }

        [Fact]
        public async Task AddTaskAsync_ShouldAddTask()
        {
            // Arrange
            var user = new User() { UserName = "Test", Id = "1" };
            var userClaims = new UserClaims { UserId = "1", HouseholdId = "1" };
            var createRequest = new CreateHouseholdTaskRequest { Title = "New Task", GroupId = 1, Priority = true, Description = "asd"};
            var task = new HouseholdTask { TaskId = 1, Title = "New Task" };

            _groupRepositoryMock.Setup(repo => repo.GetGroupByIdAsync(1)).ReturnsAsync(new TaskGroup
            {
                GroupId = 1, Household = new Household { HouseholdId = 1, CreatedByUser = user}
            });
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync("1")).ReturnsAsync(new User
            {
                Id = "1", Household = new Household { HouseholdId = 1, CreatedByUser = user}
            });
            _householdRepositoryMock.Setup(repo => repo.GetHouseholdByIdAsync(1)).ReturnsAsync(new Household
            {
                HouseholdId = 1,
                CreatedByUser = user
            });
            _householdTaskRepositoryMock.Setup(repo => repo.AddTaskAsync(It.IsAny<HouseholdTask>())).ReturnsAsync(task);

            // Act
            var result = await _householdTaskService.AddTaskAsync(createRequest, userClaims);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(task.Title, result.Title);
        }

        [Fact]
        public async Task UpdateTaskAsync_ShouldUpdateTask()
        {
            // Arrange
            var user = new User() { UserName = "Test", Id = "1" };
            var userClaims = new UserClaims { UserId = "1", HouseholdId = "1" };
            var updateRequest = new CreateHouseholdTaskRequest { Title = "Updated Task", GroupId = 1, Priority = true };
            var task = new HouseholdTask { TaskId = 1, Title = "Updated Task" };
            _groupRepositoryMock.Setup(repo => repo.GetGroupByIdAsync(1)).ReturnsAsync(new TaskGroup
            {
                GroupId = 1, Household = new Household { HouseholdId = 1, CreatedByUser = user }
            });
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync("1")).ReturnsAsync(new User
            {
                Id = "1", Household = new Household { HouseholdId = 1, CreatedByUser = user }
            });
            _householdRepositoryMock.Setup(repo => repo.GetHouseholdByIdAsync(1)).ReturnsAsync(new Household
            {
                HouseholdId = 1,
                CreatedByUser = user
            });
            _householdTaskRepositoryMock.Setup(repo => repo.UpdateTaskAsync(It.IsAny<HouseholdTask>(), 1)).ReturnsAsync(task);

            // Act
            var result = await _householdTaskService.UpdateTaskAsync(updateRequest, userClaims, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(task.Title, result.Title);
        }

        [Fact]
        public async Task DeleteTaskByIdAsync_ShouldDeleteTask()
        {
            // Arrange
            _householdTaskRepositoryMock.Setup(repo => repo.DeleteTaskByIdAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _householdTaskService.DeleteTaskByIdAsync(1);

            // Assert
            _householdTaskRepositoryMock.Verify(repo => repo.DeleteTaskByIdAsync(1), Times.Once);
        }
    }
}
