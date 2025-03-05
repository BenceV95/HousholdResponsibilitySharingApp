using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseholdResponsibilityAppServer.Models.Households;
using HouseholdResponsibilityAppServer.Models.ScheduledTasks;
using HouseholdResponsibilityAppServer.Models.Task;
using HouseholdResponsibilityAppServer.Models.Users;
using HouseholdResponsibilityAppServer.Repositories.HouseholdTasks;
using HouseholdResponsibilityAppServer.Repositories.ScheduledTasks;
using HouseholdResponsibilityAppServer.Repositories.UserRepo;
using HouseholdResponsibilityAppServer.Services.Authentication;
using HouseholdResponsibilityAppServer.Services.ScheduledTaskServices;
using Moq;
using Xunit;

namespace HouseholdResponsibilityAppServerTests.UnitTests.Services;
public class ScheduledTaskServiceTests
{
    private readonly Mock<IHouseholdTasksRepository> _mockHouseholdTaskRepo;
    private readonly Mock<IScheduledTasksRepository> _mockScheduledTaskRepo;
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly ScheduledTaskService _service;

    public ScheduledTaskServiceTests()
    {
        _mockHouseholdTaskRepo = new Mock<IHouseholdTasksRepository>();
        _mockScheduledTaskRepo = new Mock<IScheduledTasksRepository>();
        _mockUserRepo = new Mock<IUserRepository>();

        _service = new ScheduledTaskService(
            _mockScheduledTaskRepo.Object,
            _mockHouseholdTaskRepo.Object,
            _mockUserRepo.Object
        );
    }

    [Fact]
    public async Task GetAllScheduledByHouseholdIdAsync_ReturnsTasks()
    {
        var userClaims = new UserClaims { HouseholdId = "1" };
        var tasks = new List<ScheduledTask>
        {
            new ScheduledTask
            {
                ScheduledTaskId = 1,
                HouseholdTask = new HouseholdTask { TaskId = 1 },
                CreatedBy = new User { Id = "user1" },
                AssignedTo = new User { Id = "user2" },
                EventDate = DateTime.Now,
                CreatedAt = DateTime.Now,
                AtSpecificTime = true,
                Repeat = Repeat.Daily
            },
            new ScheduledTask
            {
                ScheduledTaskId = 2,
                HouseholdTask = new HouseholdTask { TaskId = 2 },
                CreatedBy = new User { Id = "user2" },
                AssignedTo = new User { Id = "user1" },
                EventDate = DateTime.Now,
                CreatedAt = DateTime.Now,
                AtSpecificTime = false,
                Repeat = Repeat.Monthly
            },
        };

        _mockScheduledTaskRepo
            .Setup(repo => repo.GetScheduledTasksByHouseholdIdAsync(1))
            .ReturnsAsync(tasks);

        var result = await _service.GetAllScheduledByHouseholdIdAsync(userClaims);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }


    [Fact]
    public async Task AddScheduledTaskAsync_AddsAndReturnsTask()
    {
        var userClaims = new UserClaims { UserId = "123", HouseholdId = "1" };
        var request = new CreateScheduledTaskRequest { HouseholdTaskId = 1, AssignedToUserId = "456" };
        var scheduledTask = new ScheduledTask { ScheduledTaskId = 1 };
        var user = new User { Id = "123" };
        var household = new Household
        {
            HouseholdId = 1,
            CreatedByUser = user,
            Users = new List<User> { user }
        };
        user.Household = household; // Ensure the user belongs to the household
        var assignedUser = new User { Id = "456", Household = household };
        var householdTask = new HouseholdTask { TaskId = 1, Household = household, CreatedBy = user };

        _mockHouseholdTaskRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(householdTask);
        _mockUserRepo.Setup(repo => repo.GetUserByIdAsync("123")).ReturnsAsync(user);
        _mockUserRepo.Setup(repo => repo.GetUserByIdAsync("456")).ReturnsAsync(assignedUser);
        _mockScheduledTaskRepo.Setup(repo => repo.AddScheduledTaskAsync(It.IsAny<ScheduledTask>())).ReturnsAsync(scheduledTask);

        var result = await _service.AddScheduledTaskAsync(request, userClaims);

        Assert.NotNull(result);
        Assert.Equal(1, result.ScheduledTaskId);
    }


    [Fact]
    public async Task DeleteScheduledTaskByIdAsync_CallsRepository()
    {
        await _service.DeleteScheduledTaskByIdAsync(1);

        _mockScheduledTaskRepo.Verify(repo => repo.DeleteScheduledTaskByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetAllScheduledTasksAsync_ReturnsAllTasks()
    {
        var tasks = new List<ScheduledTask> { new ScheduledTask { ScheduledTaskId = 1 } };

        _mockScheduledTaskRepo.Setup(repo => repo.GetAllScheduledTasksAsync()).ReturnsAsync(tasks);

        var result = await _service.GetAllScheduledTasksAsync();

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsTask()
    {
        var task = new ScheduledTask { ScheduledTaskId = 1 };

        _mockScheduledTaskRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(task);

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.ScheduledTaskId);
    }
}
