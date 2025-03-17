using HouseholdResponsibilityAppServer.Controllers;
using HouseholdResponsibilityAppServer.Models.Histories;
using HouseholdResponsibilityAppServer.Services.HistoryServices;
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
    public class HistoryControllerUnitTests
    {
        private readonly Mock<IHistoryService> _mockHistoryService;
        private readonly Mock<ILogger<GroupController>> _mockLogger;
        private readonly HistoryController _controller;

        public HistoryControllerUnitTests()
        {
            _mockHistoryService = new Mock<IHistoryService>();
            _mockLogger = new Mock<ILogger<GroupController>>();
            _controller = new HistoryController(_mockHistoryService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllHistories_ReturnsOkResult_WithListOfHistories()
        {
            // Arrange
            var histories = new List<HistoryDTO> { new HistoryDTO { HistoryId = 1, ScheduledTaskId = 1, CompletedAt = DateTime.Now, CompletedByUserId = "1", Outcome = true, HouseholdId = 1 } };
            _mockHistoryService.Setup(service => service.GetallHistoriesAsync()).ReturnsAsync(histories);

            // Act
            var result = await _controller.GetAllHistories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<HistoryDTO>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetHistoryById_ReturnsOkResult_WithHistory()
        {
            // Arrange
            var history = new HistoryDTO { HistoryId = 1, ScheduledTaskId = 1, CompletedAt = DateTime.Now, CompletedByUserId = "1", Outcome = true, HouseholdId = 1 };
            _mockHistoryService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(history);

            // Act
            var result = await _controller.GetHistoryById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<HistoryDTO>(okResult.Value);
            Assert.Equal(1, returnValue.HistoryId);
        }

        [Fact]
        public async Task PostNewHistory_ReturnsOkResult_WithHistoryId()
        {
            // Arrange
            var createRequest = new CreateHistoryRequest { ScheduledTaskId = 1, CompletedAt = DateTime.Now, CompletedByUserId = "1", Outcome = true, HouseholdId = 1 };
            var history = new HistoryDTO { HistoryId = 1, ScheduledTaskId = 1, CompletedAt = DateTime.Now, CompletedByUserId = "1", Outcome = true, HouseholdId = 1 };
            _mockHistoryService.Setup(service => service.AddHistoryAsync(createRequest)).ReturnsAsync(history);

            // Act
            var result = await _controller.PostNewHistory(createRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<int>(okResult.Value);
            Assert.Equal(1, returnValue);
        }

        [Fact]
        public async Task UpdateHistory_ReturnsOkResult_WithUpdatedHistory()
        {
            // Arrange
            var updateRequest = new CreateHistoryRequest { ScheduledTaskId = 1, CompletedAt = DateTime.Now, CompletedByUserId = "1", Outcome = true, HouseholdId = 1 };
            var history = new HistoryDTO { HistoryId = 1, ScheduledTaskId = 1, CompletedAt = DateTime.Now, CompletedByUserId = "1", Outcome = true, HouseholdId = 1 };
            _mockHistoryService.Setup(service => service.UpdateHistoryAsync(updateRequest)).ReturnsAsync(history);

            // Act
            var result = await _controller.UpdateHistory(updateRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<HistoryDTO>(okResult.Value);
            Assert.Equal(1, returnValue.HistoryId);
        }

        [Fact]
        public async Task DeleteHistory_ReturnsNoContentResult()
        {
            // Arrange
            _mockHistoryService.Setup(service => service.DeleteHistoryByIdAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteHistory(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
