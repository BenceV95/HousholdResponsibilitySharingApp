using HouseholdResponsibilityAppServer.Models.Histories;
using HouseholdResponsibilityAppServer.Models.HouseholdTasks;
using HouseholdResponsibilityAppServer.Repositories.Histories;
using HouseholdResponsibilityAppServer.Repositories.ScheduledTasks;
using System.Threading.Tasks;

namespace HouseholdResponsibilityAppServer.Services.HistoryServices
{
    public class HistoryService : IHistoryService
    {

        private readonly IHistoryRepository _historiesRepository;
        private readonly IUserService _userService;
        private readonly IScheduledTasksRepository _scheduledTasksRepository;
        public HistoryService(IUserService userService, IScheduledTasksRepository scheduledTasksRepository, IHistoryRepository historiesRepository)
        {
            _userService = userService;
            _scheduledTasksRepository = scheduledTasksRepository;
            _historiesRepository = historiesRepository;
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
            List<HistoryDTO> historyDTOs = new List<HistoryDTO>();
            var historyModels = await _historiesRepository.GetAllHistoriesAsync();
            foreach (var history in historyModels)
            {
                historyDTOs.Add(ConvertModelToDTO(history));
            }
            return historyDTOs;
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
            var completedBy = await _userService.GetUserByIdAsync(historyCreateRequest.CompletedByUserId);

            return new History()
            {
                ScheduledTask = scheduledTask,
                Action = historyCreateRequest.Action,
                CompletedAt = historyCreateRequest.CompletedAt,
                CompletedBy = completedBy,
            };
        }
        private HistoryDTO ConvertModelToDTO(History historyModel)
        {
            return new HistoryDTO()
            {
                HistoryId = historyModel.HistoryId,
                CompletedByUserId = historyModel.CompletedBy.UserId,
                CompletedAt = historyModel.CompletedAt,
                ScheduledTaskId = historyModel.ScheduledTask.ScheduledTaskId,
                Action = historyModel.Action,
            };
        }

    }
}
