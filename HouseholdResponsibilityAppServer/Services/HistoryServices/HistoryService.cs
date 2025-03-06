using HouseholdResponsibilityAppServer.Models.Histories;
using HouseholdResponsibilityAppServer.Repositories.Histories;
using HouseholdResponsibilityAppServer.Repositories.ScheduledTasks;
using HouseholdResponsibilityAppServer.Repositories.UserRepo;
using HouseholdResponsibilityAppServer.Services.UserService;
using System.Threading.Tasks;

namespace HouseholdResponsibilityAppServer.Services.HistoryServices
{
    public class HistoryService : IHistoryService
    {
        private readonly IHistoryRepository _historiesRepository;
        private readonly IScheduledTasksRepository _scheduledTasksRepository;
        private readonly IUserRepository _userRepository;

        public HistoryService( IScheduledTasksRepository scheduledTasksRepository, IHistoryRepository historiesRepository, IUserRepository userRepository)
        {
            _scheduledTasksRepository = scheduledTasksRepository;
            _historiesRepository = historiesRepository;
            _userRepository = userRepository;
        }

        public async Task<HistoryDTO> AddHistoryAsync(CreateHistoryRequest historyCreateRequest)
        {
            var historyModel = await ConvertRequestToModel(historyCreateRequest);

            var addedModel = await _historiesRepository.AddHistoryAsync(historyModel);

            return ConvertModelToDTO(addedModel);
        }

        public async Task DeleteHistoryByIdAsync(int historyId)
        {
            await _historiesRepository.DeleteHistoryByIdAsync(historyId);
        }

        public async Task<IEnumerable<HistoryDTO>> GetallHistoriesAsync()
        {
            var historyModels = await _historiesRepository.GetAllHistoriesAsync();
            return historyModels.Select(ConvertModelToDTO);
        }

        public async Task<HistoryDTO> GetByIdAsync(int historyId)
        {
            var historyModel = await _historiesRepository.GetByIdAsync(historyId);
            return ConvertModelToDTO(historyModel);
        }

        public async Task<HistoryDTO> UpdateHistoryAsync(CreateHistoryRequest updateRequest)
        {
            var historyModel = await ConvertRequestToModel(updateRequest);

            var updatedModel = await _historiesRepository.UpdateHistoryAsync(historyModel);
            return ConvertModelToDTO(updatedModel);
        }

        private async Task<History> ConvertRequestToModel(CreateHistoryRequest historyCreateRequest)
        {
            var scheduledTask = await _scheduledTasksRepository.GetByIdAsync(historyCreateRequest.ScheduledTaskId);
            if (scheduledTask == null)
            {
                throw new KeyNotFoundException("Scheduled task not found !");
            }

            var completedBy = await _userRepository.GetUserByIdAsync(historyCreateRequest.CompletedByUserId);
            if (completedBy == null)
            {
                throw new KeyNotFoundException("User not found !");
            }

            return new History()
            {
                ScheduledTask = scheduledTask,
                Outcome = historyCreateRequest.Outcome,
                CompletedAt = historyCreateRequest.CompletedAt,
                CompletedBy = completedBy,
            };
        }

        private HistoryDTO ConvertModelToDTO(History historyModel)
        {
            return new HistoryDTO()
            {
                HistoryId = historyModel.HistoryId,
                CompletedByUserId = historyModel.CompletedBy?.Id ?? string.Empty,
                CompletedAt = historyModel.CompletedAt,
                ScheduledTaskId = historyModel.ScheduledTask?.ScheduledTaskId ?? 0,
                Outcome = historyModel.Outcome,

            };
        }
    }
}
