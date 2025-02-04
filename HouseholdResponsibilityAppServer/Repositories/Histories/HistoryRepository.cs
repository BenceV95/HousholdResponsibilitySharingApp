using HouseholdResponsibilityAppServer.Context;
using HouseholdResponsibilityAppServer.Models.Histories;
using HouseholdResponsibilityAppServer.Models.Task;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
            History history = new() { HistoryId = historyId };

            _dbContext.Entry(history).State = EntityState.Deleted;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<History>> GetAllHistoriesAsync()
        {
            return await _dbContext.Histories.ToListAsync();
        }

        public async Task<History> GetByIdAsync(int historyId)
        {
            var history = await _dbContext.Histories.FindAsync(historyId);
            return history ?? throw new KeyNotFoundException("No history entry was found with given Id!");
        }

        public async Task<History> UpdateHistoryAsync(History history)
        {
            var existingHistory = await _dbContext.Histories.FindAsync(history.HistoryId);
            if (existingHistory == null)
            {
                throw new KeyNotFoundException("Couldn't find history entry in the db to update!");
            }

            existingHistory.Action = history.Action;
            existingHistory.CompletedBy = history.CompletedBy;
            existingHistory.CompletedAt = history.CompletedAt;


            await _dbContext.SaveChangesAsync();

            return existingHistory;
        }
    }
}
