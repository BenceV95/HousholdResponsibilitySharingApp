using HouseholdResponsibilityAppServer.Models.ScheduledTasks;

namespace HouseholdResponsibilityAppServer.Repositories.ScheduledTasks
{
    public interface IScheduledTasksRepository
    {
        public Task<ScheduledTask> AddScheduledTaskAsync(ScheduledTask scheduledTask);
        public Task<IEnumerable<ScheduledTask>> GetAllScheduledTasksAsync();
        public Task DeleteScheduledTaskByIdAsync(int scheduledTaskId);
        public Task<ScheduledTask> GetByIdAsync(int scheduledTaskId);
        public Task<ScheduledTask> UpdateSheduledTaskAsync(ScheduledTask scheduledTask, int taskId);
        public Task<IEnumerable<ScheduledTask>> GetScheduledTasksByHouseholdIdAsync(int householdId);
    }
}
