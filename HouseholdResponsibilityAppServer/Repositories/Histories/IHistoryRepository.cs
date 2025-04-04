﻿using HouseholdResponsibilityAppServer.Models.Histories;

namespace HouseholdResponsibilityAppServer.Repositories.Histories
{
    public interface IHistoryRepository
    {
        public Task<History> AddHistoryAsync(History householdTask);
        public Task<IEnumerable<History>> GetAllHistoriesAsync();
        public Task<IEnumerable<History>> GetAllHistoriesAsync(int householdId);
        public Task DeleteHistoryByIdAsync(int historyId);
        public Task<History> GetByIdAsync(int historyId);
        //not sure if this will be needed
        public Task<History> UpdateHistoryAsync(History householdTask);
    }
}
