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
            return await _dbContext.Histories
                .Include(h => h.CompletedBy)
                .Include(h => h.Household)
                .Include(h => h.ScheduledTask)
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
