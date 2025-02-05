using HouseholdResponsibilityAppServer.Models.ScheduledTasks;
using HouseholdResponsibilityAppServer.Models.Task;

namespace HouseholdResponsibilityAppServer.Repositories.ScheduledTasks
{
    public interface IScheduledTasksRepository
    {
        public Task<ScheduledTask> AddScheduledTaskAsync(ScheduledTask scheduledTask);
        public Task<IEnumerable<ScheduledTask>> GetAllScheduledTasksAsync();
        public Task DeleteScheduledTaskByIdAsync(int scheduledTaskId);
        public Task<ScheduledTask> GetByIdAsync(int scheduledTaskId);
        public Task<ScheduledTask> UpdateSheduledTaskAsync(ScheduledTask scheduledTask);
    }
}
