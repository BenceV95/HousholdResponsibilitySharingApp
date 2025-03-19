using HouseholdResponsibilityAppServer.Models.Histories;
using HouseholdResponsibilityAppServer.Models.ScheduledTasks;
using HouseholdResponsibilityAppServer.Models.Users;
using HouseholdResponsibilityAppServer.Repositories.Histories;
using HouseholdResponsibilityAppServer.Repositories.ScheduledTasks;
using HouseholdResponsibilityAppServer.Repositories.UserRepo;
using HouseholdResponsibilityAppServer.Services.HistoryServices;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HouseholdResponsibilityAppServerTests.UnitTests.Services
{
    public class HistoryServiceUnitTests
    {
        private readonly Mock<IHistoryRepository> _mockHistoryRepository;
        private readonly Mock<IScheduledTasksRepository> _mockScheduledTasksRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly HistoryService _historyService;

        public HistoryServiceUnitTests()
        {
            _mockHistoryRepository = new Mock<IHistoryRepository>();
            _mockScheduledTasksRepository = new Mock<IScheduledTasksRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _historyService = new HistoryService(
                _mockScheduledTasksRepository.Object,
                _mockHistoryRepository.Object,
                _mockUserRepository.Object);
        }

        [Fact]
        public async Task AddHistoryAsync_ShouldAddHistory()
        {
            // Arrange
            var createRequest = new CreateHistoryRequest
            {
                ScheduledTaskId = 1,
                CompletedAt = DateTime.Now,
                CompletedByUserId = "user1",
                Outcome = true
            };

            var scheduledTask = new ScheduledTask { ScheduledTaskId = 1 };
            var user = new User { Id = "user1" };
            var history = new History
            {
                ScheduledTask = scheduledTask,
                CompletedAt = createRequest.CompletedAt,
                CompletedBy = user,
                Outcome = createRequest.Outcome
            };

            _mockScheduledTasksRepository.Setup(
                repo => 
                    repo.GetByIdAsync(createRequest.ScheduledTaskId))
                    .ReturnsAsync(scheduledTask);

            _mockUserRepository.Setup(
                repo => 
                    repo.GetUserByIdAsync(createRequest.CompletedByUserId))
                    .ReturnsAsync(user);

            _mockHistoryRepository.Setup(
                repo => 
                    repo.AddHistoryAsync(It.IsAny<History>()))
                    .ReturnsAsync(history);

            // Act
            var result = await _historyService.AddHistoryAsync(createRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createRequest.CompletedByUserId, result.CompletedByUserId);
            _mockHistoryRepository.Verify(
                repo => 
                    repo.AddHistoryAsync(It.IsAny<History>()), Times.Once);
        }

        [Fact]
        public async Task DeleteHistoryByIdAsync_ShouldDeleteHistory()
        {
            // Arrange
            var historyId = 1;

            // Act
            await _historyService.DeleteHistoryByIdAsync(historyId);

            // Assert
            _mockHistoryRepository.Verify(repo => repo.DeleteHistoryByIdAsync(historyId), Times.Once);
        }

        [Fact]
        public async Task GetAllHistoriesAsync_ShouldReturnAllHistories()
        {
            // Arrange
            var histories = new List<History>
            {
                new History { HistoryId = 1 },
                new History { HistoryId = 2 }
            };

            _mockHistoryRepository.Setup(repo => repo.GetAllHistoriesAsync()).ReturnsAsync(histories);

            // Act
            var result = await _historyService.GetallHistoriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockHistoryRepository.Verify(repo => repo.GetAllHistoriesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnHistoryById()
        {
            // Arrange
            var historyId = 1;
            var history = new History { HistoryId = historyId };

            _mockHistoryRepository.Setup(repo => repo.GetByIdAsync(historyId)).ReturnsAsync(history);

            // Act
            var result = await _historyService.GetByIdAsync(historyId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(historyId, result.HistoryId);
            _mockHistoryRepository.Verify(repo => repo.GetByIdAsync(historyId), Times.Once);
        }

        [Fact]
        public async Task UpdateHistoryAsync_ShouldUpdateHistory()
        {
            // Arrange
            var user = new User { Id = "user1" };
            var updateRequest = new UpdateHistoryDTO(1, true, "user1");
            var history = new History
            {
                HistoryId = 1,
                CompletedBy = user,
                Outcome = updateRequest.Outcome
            };

            _mockHistoryRepository.Setup(
                repo =>
                    repo.GetByIdAsync(updateRequest.Id))
                .ReturnsAsync(history);

            _mockUserRepository.Setup(
                repo => 
                    repo.GetUserByIdAsync(updateRequest.CompletedByUserId))
                .ReturnsAsync(user);

            _mockHistoryRepository.Setup(
                repo => 
                    repo.UpdateHistoryAsync(It.IsAny<History>()))
                .ReturnsAsync(history);

            // Act
            var result = await _historyService.UpdateHistoryAsync(updateRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updateRequest.Outcome, result.Outcome);
            _mockHistoryRepository.Verify(
                repo => 
                    repo.UpdateHistoryAsync(It.IsAny<History>()), Times.Once);
        }
    }
}
