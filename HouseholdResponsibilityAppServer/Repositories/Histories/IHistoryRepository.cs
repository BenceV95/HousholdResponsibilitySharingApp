using HouseholdResponsibilityAppServer.Models.Histories;
using HouseholdResponsibilityAppServer.Models.Task;

namespace HouseholdResponsibilityAppServer.Repositories.Histories
{
    public interface IHistoryRepository
    {
        public Task<History> AddHistoryAsync(History householdTask);
        public Task<IEnumerable<History>> GetAllHistoriesAsync();
        public Task DeleteHistoryByIdAsync(int historyId);
        public Task<History> GetByIdAsync(int historyId);
        //not sure if this will be needed
        public Task<History> UpdateHistoryAsync(HouseholdTask householdTask);
    }
}
