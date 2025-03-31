using HouseholdResponsibilityAppServer.Models.Histories;

namespace HouseholdResponsibilityAppServer.Services.HistoryServices
{
    public interface IHistoryService
    {
        public Task<HistoryDTO> AddHistoryAsync(CreateHistoryRequest historyCreateRequest);

        public Task<IEnumerable<HistoryDTO>> GetallHistoriesAsync();

        public Task<IEnumerable<HistoryDTO>> GetallHistoriesAsync(int householdId);

        public Task DeleteHistoryByIdAsync(int historyId);

        public Task<HistoryDTO> GetByIdAsync(int historyId);

        public Task<HistoryDTO> UpdateHistoryAsync(UpdateHistoryDTO updateRequest);
    }
}