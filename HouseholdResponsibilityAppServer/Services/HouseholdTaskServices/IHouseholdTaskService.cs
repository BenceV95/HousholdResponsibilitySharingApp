using HouseholdResponsibilityAppServer.Models.HouseholdTasks;

namespace HouseholdResponsibilityAppServer.Services.HouseholdTaskServices
{
    public interface IHouseholdTaskService
    {

        public Task<HouseholdTaskDTO> AddTaskAsync(CreateHouseholdTaskRequest taskCreateRequest);
        public Task<IEnumerable<HouseholdTaskDTO>> GetallTasksAsync();

        public Task DeleteTaskByIdAsync(int taskId);

        public Task<HouseholdTaskDTO> GetByIdAsync(int taskId);

        public Task<HouseholdTaskDTO> UpdateTaskAsync(CreateHouseholdTaskRequest updateRequest);
    }
}
