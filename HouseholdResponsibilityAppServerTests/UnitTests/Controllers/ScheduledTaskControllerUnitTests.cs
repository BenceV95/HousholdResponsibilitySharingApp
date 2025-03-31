using HouseholdResponsibilityAppServer.Controllers;
using HouseholdResponsibilityAppServer.Models.ScheduledTasks;
using HouseholdResponsibilityAppServer.Services.Authentication;
using HouseholdResponsibilityAppServer.Services.ScheduledTaskServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HouseholdResponsibilityAppServerTests.UnitTests.Controllers
{
    public class ScheduledTaskControllerUnitTests
    {
        private readonly Mock<IScheduledTaskService> _mockScheduledTaskService;
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<ILogger<GroupController>> _mockLogger;
        private readonly ScheduledTaskController _controller;

        public ScheduledTaskControllerUnitTests()
        {
            _mockScheduledTaskService = new Mock<IScheduledTaskService>();
            _mockAuthService = new Mock<IAuthService>();
            _mockLogger = new Mock<ILogger<GroupController>>();
            _controller = new ScheduledTaskController(_mockScheduledTaskService.Object, _mockAuthService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllTasks_ReturnsOkResult_WithListOfScheduledTasks()
        {
            // Arrange
            var scheduledTasks = new List<ScheduledTaskDTO> { new ScheduledTaskDTO { ScheduledTaskId = 1, HouseholdTaskId = 1, CreatedByUserId = "1", CreatedAt = DateTime.Now, Repeat = Repeat.Daily, EventDate = DateTime.Now, AtSpecificTime = true, AssignedToUserId = "1" } };
            _mockScheduledTaskService.Setup(service => service.GetAllScheduledTasksAsync()).ReturnsAsync(scheduledTasks);

            // Act
            var result = await _controller.GetAllTasks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ScheduledTaskDTO>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetTaskById_ReturnsOkResult_WithScheduledTask()
        {
            // Arrange
            var scheduledTask = new ScheduledTaskDTO { ScheduledTaskId = 1, HouseholdTaskId = 1, CreatedByUserId = "1", CreatedAt = DateTime.Now, Repeat = Repeat.Daily, EventDate = DateTime.Now, AtSpecificTime = true, AssignedToUserId = "1" };
            _mockScheduledTaskService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(scheduledTask);

            // Act
            var result = await _controller.GetTaskById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ScheduledTaskDTO>(okResult.Value);
            Assert.Equal(1, returnValue.ScheduledTaskId);
        }

        [Fact]
        public async Task PostNewTask_ReturnsOkResult_WithScheduledTaskId()
        {
            // Arrange
            var createRequest = new CreateScheduledTaskRequest { HouseholdTaskId = 1, Repeat = Repeat.Daily, EventDate = DateTime.Now, AtSpecificTime = true, AssignedToUserId = "1" };
            var userClaims = new UserClaims { UserId = "1", UserName = "TestUser", Email = "test@example.com", HouseholdId = "1", Roles = new List<string> { "User" } };
            var scheduledTask = new ScheduledTaskDTO { ScheduledTaskId = 1, HouseholdTaskId = 1, CreatedByUserId = "1", CreatedAt = DateTime.Now, Repeat = Repeat.Daily, EventDate = DateTime.Now, AtSpecificTime = true, AssignedToUserId = "1" };
            _mockAuthService.Setup(service => service.GetClaimsFromHttpContext(It.IsAny<HttpContext>())).Returns(userClaims);
            _mockScheduledTaskService.Setup(service => service.AddScheduledTaskAsync(createRequest, userClaims)).ReturnsAsync(scheduledTask);

            // Act
            var result = await _controller.PostNewTask(createRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<int>(okResult.Value);
            Assert.Equal(1, returnValue);
        }

        [Fact]
        public async Task UpdateTask_ReturnsOkResult_WithUpdatedScheduledTask()
        {
            // Arrange
            var updateRequest = new CreateScheduledTaskRequest { HouseholdTaskId = 1, Repeat = Repeat.Daily, EventDate = DateTime.Now, AtSpecificTime = true, AssignedToUserId = "1" };
            var userClaims = new UserClaims { UserId = "1", UserName = "TestUser", Email = "test@example.com", HouseholdId = "1", Roles = new List<string> { "User" } };
            var scheduledTask = new ScheduledTaskDTO { ScheduledTaskId = 1, HouseholdTaskId = 1, CreatedByUserId = "1", CreatedAt = DateTime.Now, Repeat = Repeat.Daily, EventDate = DateTime.Now, AtSpecificTime = true, AssignedToUserId = "1" };
            _mockAuthService.Setup(service => service.GetClaimsFromHttpContext(It.IsAny<HttpContext>())).Returns(userClaims);
            _mockScheduledTaskService.Setup(service => service.UpdateScheduledTaskAsync(updateRequest, userClaims, 1)).ReturnsAsync(scheduledTask);

            // Act
            var result = await _controller.UpdateTask(updateRequest, 1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ScheduledTaskDTO>(okResult.Value);
            Assert.Equal(1, returnValue.ScheduledTaskId);
        }

        [Fact]
        public async Task DeleteScheduledTask_ReturnsNoContentResult()
        {
            // Arrange
            _mockScheduledTaskService.Setup(service => service.DeleteScheduledTaskByIdAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteScheduledTask(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetAllScheduledsByHousehold_ReturnsOkResult_WithListOfScheduledTasks()
        {
            // Arrange
            var scheduledTasks = new List<ScheduledTaskDTO> { new ScheduledTaskDTO { ScheduledTaskId = 1, HouseholdTaskId = 1, CreatedByUserId = "1", CreatedAt = DateTime.Now, Repeat = Repeat.Daily, EventDate = DateTime.Now, AtSpecificTime = true, AssignedToUserId = "1" } };
            var userClaims = new UserClaims { UserId = "1", UserName = "TestUser", Email = "test@example.com", HouseholdId = "1", Roles = new List<string> { "User" } };
            _mockAuthService.Setup(service => service.GetClaimsFromHttpContext(It.IsAny<HttpContext>())).Returns(userClaims);
            _mockScheduledTaskService.Setup(service => service.GetAllScheduledByHouseholdIdAsync(userClaims)).ReturnsAsync(scheduledTasks);

            // Act
            var result = await _controller.GetAllScheduledsByHousehold();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ScheduledTaskDTO>>(okResult.Value);
            Assert.Single(returnValue);
        }
    }
}
