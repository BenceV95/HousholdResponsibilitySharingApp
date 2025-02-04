using HouseholdResponsibilityAppServer.Models.HouseholdTasks;
using HouseholdResponsibilityAppServer.Models.ScheduledTask;

namespace HouseholdResponsibilityAppServer.Services.ScheduledTaskServices
{
    public interface IScheduledTaskService
    {
        public Task<ScheduledTaskDTO> AddScheduledTaskAsync(CreateScheduledTaskRequest scheduledTaskCreateRequest);
        public Task<IEnumerable<ScheduledTaskDTO>> GetAllScheduledTasksAsync();

        public Task DeleteScheduledTaskByIdAsync(int scheduledTaskId);

        public Task<ScheduledTaskDTO> GetByIdAsync(int scheduledTaskId);

        public Task<ScheduledTaskDTO> UpdateScheduledTaskAsync(CreateScheduledTaskRequest updateRequest);
    }
}
