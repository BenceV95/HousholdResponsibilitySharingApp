using HouseholdResponsibilityAppServer.Models.Households;
using HouseholdResponsibilityAppServer.Models.HouseholdTasks;
using HouseholdResponsibilityAppServer.Models.Task;

namespace HouseholdResponsibilityAppServer.Services.HouseholdTaskServices
{
    public interface IHouseholdTaskService
    {

        public Task<HouseholdTaskDTO> AddTaskAsync(CreateHouseholdTaskRequest taskCreateRequest);
        public Task<IEnumerable<HouseholdTaskDTO>> GetallTasksAsync();

        public Task DeleteTaskByIdAsync(int taskId);

        public Task<HouseholdTaskDTO> GetByIdAsync(int taskId);

        public Task<HouseholdTaskDTO> UpdateTaskAsync(CreateHouseholdTaskRequest updateRequest, int id);

        public Task<IEnumerable<HouseholdTaskDTO>> GetallTasksByHouseholdIdAsync(int householdId);
    }
}
