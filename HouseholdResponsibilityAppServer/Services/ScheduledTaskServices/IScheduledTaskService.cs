using HouseholdResponsibilityAppServer.Models.ScheduledTasks;
using HouseholdResponsibilityAppServer.Services.Authentication;

namespace HouseholdResponsibilityAppServer.Services.ScheduledTaskServices
{
    public interface IScheduledTaskService
    {
        public Task<ScheduledTaskDTO> AddScheduledTaskAsync(CreateScheduledTaskRequest scheduledTaskCreateRequest, UserClaims userClaims);
        public Task<IEnumerable<ScheduledTaskDTO>> GetAllScheduledTasksAsync();

        public Task DeleteScheduledTaskByIdAsync(int scheduledTaskId);

        public Task<ScheduledTaskDTO> GetByIdAsync(int scheduledTaskId);

        public Task<ScheduledTaskDTO> UpdateScheduledTaskAsync(CreateScheduledTaskRequest updateRequest, UserClaims userClaims, int taskId);
        public Task<IEnumerable<ScheduledTaskDTO>> GetAllScheduledByHouseholdIdAsync(UserClaims userClaims);
    }
}
