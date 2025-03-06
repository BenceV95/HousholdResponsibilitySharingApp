using HouseholdResponsibilityAppServer.Context;
using HouseholdResponsibilityAppServer.Models.ScheduledTasks;
using HouseholdResponsibilityAppServer.Models.Task;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HouseholdResponsibilityAppServer.Repositories.ScheduledTasks
{
    public class ScheduledTasksRepository : IScheduledTasksRepository
    {

        private HouseholdResponsibilityAppContext _dbContext;
        public ScheduledTasksRepository(HouseholdResponsibilityAppContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ScheduledTask> AddScheduledTaskAsync(ScheduledTask scheduledTask)
        {
            await _dbContext.ScheduledTasks.AddAsync(scheduledTask);
            await _dbContext.SaveChangesAsync();
            return scheduledTask;
        }

        public async Task DeleteScheduledTaskByIdAsync(int scheduledTaskId)
        {
            ScheduledTask scheduledTask = new() { ScheduledTaskId = scheduledTaskId };

            _dbContext.Entry(scheduledTask).State = EntityState.Deleted;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ScheduledTask>> GetAllScheduledTasksAsync()
        {
            return _dbContext.ScheduledTasks
                .Include(task => task.CreatedBy)
                .Include(task => task.HouseholdTask)
                .ThenInclude(ht => ht.Household)
                .AsQueryable();
        }

        public async Task<ScheduledTask> GetByIdAsync(int scheduledTaskId)
        {
            var scheduledTask = await _dbContext.ScheduledTasks
                .Include(t => t.AssignedTo)
                .Include(t => t.HouseholdTask)
                .Include(t => t.CreatedBy)
                .SingleOrDefaultAsync(t => t.ScheduledTaskId == scheduledTaskId);

            return scheduledTask ?? throw new KeyNotFoundException("No scheduled task was found with given Id!");
        }

        public async Task<ScheduledTask> UpdateSheduledTaskAsync(ScheduledTask scheduledTask, int taskId)
        {
            var existingTask = await _dbContext.ScheduledTasks.FindAsync(taskId);
            if (existingTask == null)
            {
                throw new KeyNotFoundException($"Couldn't find scheduled task in the db to update!");
            }

            existingTask.EventDate = scheduledTask.EventDate;
            existingTask.Repeat = scheduledTask.Repeat;
            existingTask.AssignedTo = scheduledTask.AssignedTo;

            //createdAt, createdBy shouldn't be updated I think

            await _dbContext.SaveChangesAsync();

            return existingTask;

        }

        public async Task<IEnumerable<ScheduledTask>> GetScheduledTasksByHouseholdIdAsync(int householdId)
        {
            return await _dbContext.ScheduledTasks
                .Include(task => task.CreatedBy)
                .Include(task => task.HouseholdTask)
                .ThenInclude(ht => ht.Household)
                .Where(task => task.HouseholdTask.Household.HouseholdId == householdId)
                .ToListAsync();
        }
    }
}
