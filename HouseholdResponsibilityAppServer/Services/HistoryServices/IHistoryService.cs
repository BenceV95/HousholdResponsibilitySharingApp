using HouseholdResponsibilityAppServer.Models.Histories;
using HouseholdResponsibilityAppServer.Models.HouseholdTasks;

namespace HouseholdResponsibilityAppServer.Services.HistoryServices
{
    public interface IHistoryService
    {
        public Task<HistoryDTO> AddHistoryAsync(CreateHistoryRequest historyCreateRequest);
        public Task<IEnumerable<HistoryDTO>> GetallHistoriesAsync();

        public Task DeleteHistoryByIdAsync(int historyId);

        public Task<HistoryDTO> GetByIdAsync(int historyId);

        public Task<HistoryDTO> UpdateHistoryAsync(CreateHistoryRequest updateRequest);
    }
}