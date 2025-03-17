using HouseholdResponsibilityAppServer.Controllers;
using HouseholdResponsibilityAppServer.Models.HouseholdTasks;
using HouseholdResponsibilityAppServer.Services.Authentication;
using HouseholdResponsibilityAppServer.Services.HouseholdTaskServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HouseholdResponsibilityAppServerTests.UnitTests.Controllers
{
    public class TaskControllerUnitTests
    {
        private readonly Mock<IHouseholdTaskService> _mockHouseholdTaskService;
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<ILogger<GroupController>> _mockLogger;
        private readonly HouseholdTaskController _controller;

        public TaskControllerUnitTests()
        {
            _mockHouseholdTaskService = new Mock<IHouseholdTaskService>();
            _mockAuthService = new Mock<IAuthService>();
            _mockLogger = new Mock<ILogger<GroupController>>();
            _controller = new HouseholdTaskController(_mockHouseholdTaskService.Object, _mockAuthService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetHouseholdTasksByUsersHousehold_ReturnsOkResult_WithListOfTasks()
        {
            // Arrange
            var userClaims = new UserClaims { UserId = "1", UserName = "TestUser", Email = "test@example.com", HouseholdId = "1", Roles = new List<string> { "User" } };
            var tasks = new List<HouseholdTaskDTO> { new HouseholdTaskDTO { TaskId = 1, Title = "Test Task", Description = "Test Description", CreatedAt = DateTime.Now, UserId = "1", GroupId = 1, Priority = true, HouseholdId = 1 } };
            _mockAuthService.Setup(service => service.GetClaimsFromHttpContext(It.IsAny<HttpContext>())).Returns(userClaims);
            _mockHouseholdTaskService.Setup(service => service.GetallTasksByHouseholdIdAsync(userClaims)).ReturnsAsync(tasks);

            // Act
            var result = await _controller.GetHouseholdTasksByUsersHousehold();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<HouseholdTaskDTO>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetAllTasks_ReturnsOkResult_WithListOfTasks()
        {
            // Arrange
            var tasks = new List<HouseholdTaskDTO> { new HouseholdTaskDTO { TaskId = 1, Title = "Test Task", Description = "Test Description", CreatedAt = DateTime.Now, UserId = "1", GroupId = 1, Priority = true, HouseholdId = 1 } };
            _mockHouseholdTaskService.Setup(service => service.GetallTasksAsync()).ReturnsAsync(tasks);

            // Act
            var result = await _controller.GetAllTasks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<HouseholdTaskDTO>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetTaskById_ReturnsOkResult_WithTask()
        {
            // Arrange
            var task = new HouseholdTaskDTO { TaskId = 1, Title = "Test Task", Description = "Test Description", CreatedAt = DateTime.Now, UserId = "1", GroupId = 1, Priority = true, HouseholdId = 1 };
            _mockHouseholdTaskService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(task);

            // Act
            var result = await _controller.GetTaskById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<HouseholdTaskDTO>(okResult.Value);
            Assert.Equal(1, returnValue.TaskId);
        }

        [Fact]
        public async Task PostNewTask_ReturnsOkResult_WithTaskId()
        {
            // Arrange
            var createRequest = new CreateHouseholdTaskRequest { Title = "New Task", Description = "New Description", GroupId = 1, Priority = true };
            var userClaims = new UserClaims { UserId = "1", UserName = "TestUser", Email = "test@example.com", HouseholdId = "1", Roles = new List<string> { "User" } };
            var task = new HouseholdTaskDTO { TaskId = 1, Title = "New Task", Description = "New Description", CreatedAt = DateTime.Now, UserId = "1", GroupId = 1, Priority = true, HouseholdId = 1 };
            _mockAuthService.Setup(service => service.GetClaimsFromHttpContext(It.IsAny<HttpContext>())).Returns(userClaims);
            _mockHouseholdTaskService.Setup(service => service.AddTaskAsync(createRequest, userClaims)).ReturnsAsync(task);

            // Act
            var result = await _controller.PostNewTask(createRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<int>(okResult.Value);
            Assert.Equal(1, returnValue);
        }

        [Fact]
        public async Task UpdateTask_ReturnsOkResult_WithUpdatedTask()
        {
            // Arrange
            var updateRequest = new CreateHouseholdTaskRequest { Title = "Updated Task", Description = "Updated Description", GroupId = 1, Priority = true };
            var userClaims = new UserClaims { UserId = "1", UserName = "TestUser", Email = "test@example.com", HouseholdId = "1", Roles = new List<string> { "User" } };
            var task = new HouseholdTaskDTO { TaskId = 1, Title = "Updated Task", Description = "Updated Description", CreatedAt = DateTime.Now, UserId = "1", GroupId = 1, Priority = true, HouseholdId = 1 };
            _mockAuthService.Setup(service => service.GetClaimsFromHttpContext(It.IsAny<HttpContext>())).Returns(userClaims);
            _mockHouseholdTaskService.Setup(service => service.UpdateTaskAsync(updateRequest, userClaims, 1)).ReturnsAsync(task);

            // Act
            var result = await _controller.UpdateTask(updateRequest, 1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<HouseholdTaskDTO>(okResult.Value);
            Assert.Equal(1, returnValue.TaskId);
        }

        [Fact]
        public async Task DeleteTask_ReturnsNoContentResult()
        {
            // Arrange
            _mockHouseholdTaskService.Setup(service => service.DeleteTaskByIdAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteTask(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}

