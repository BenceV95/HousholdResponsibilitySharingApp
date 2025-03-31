using HouseholdResponsibilityAppServer.Context;
using HouseholdResponsibilityAppServer.Models.Histories;
using Microsoft.EntityFrameworkCore;

namespace HouseholdResponsibilityAppServer.Repositories.Histories
{
    public class HistoryRepository : IHistoryRepository
    {

        private HouseholdResponsibilityAppContext _dbContext;
        public HistoryRepository(HouseholdResponsibilityAppContext dbContext)
        {
            _dbContext = dbContext;
        }



        public async Task<History> AddHistoryAsync(History history)
        {
            await _dbContext.Histories.AddAsync(history);
            await _dbContext.SaveChangesAsync();
            return history;
        }

        public async Task DeleteHistoryByIdAsync(int historyId)
        {
            var history = await _dbContext.Histories.FirstOrDefaultAsync(h=>h.HistoryId == historyId);

            if (history == null)
            {
                throw new KeyNotFoundException($"History with ID {historyId} not found.");
            }

            _dbContext.Histories.Remove(history);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<History>> GetAllHistoriesAsync()
        {
            return await _dbContext.Histories
                .Include(h => h.CompletedBy)
                .Include(h => h.Household)
                .Include(h => h.ScheduledTask)
                .ToListAsync();
        }

        public async Task<IEnumerable<History>> GetAllHistoriesAsync(int householdId)
        {
            return await _dbContext.Histories
                .Include(h => h.CompletedBy)
                .Include(h => h.Household)
                .Include(h => h.ScheduledTask)
                .Where(h=>h.Household.HouseholdId == householdId)
                .ToListAsync();
        }

        public async Task<History> GetByIdAsync(int historyId)
        {
            var history = await _dbContext.Histories
                .Include(h => h.CompletedBy)
                .Include(h => h.Household)
                .Include(h => h.ScheduledTask)
                .SingleOrDefaultAsync(h => h.HistoryId == historyId );
            return history ?? throw new KeyNotFoundException("No history entry was found with given Id!");
        }

        public async Task<History> UpdateHistoryAsync(History history)
        {
            var existingHistory = await _dbContext.Histories.FindAsync(history.HistoryId);
            if (existingHistory == null)
            {
                throw new KeyNotFoundException("Couldn't find history entry in the db to update!");
            }

            existingHistory.Outcome = history.Outcome;
            existingHistory.CompletedBy = history.CompletedBy;
            existingHistory.CompletedAt = history.CompletedAt;


            await _dbContext.SaveChangesAsync();

            return existingHistory;
        }
    }
}
