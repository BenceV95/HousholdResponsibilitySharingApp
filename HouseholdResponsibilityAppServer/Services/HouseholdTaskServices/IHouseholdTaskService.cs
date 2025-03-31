using HouseholdResponsibilityAppServer.Models.HouseholdTasks;
using HouseholdResponsibilityAppServer.Services.Authentication;

namespace HouseholdResponsibilityAppServer.Services.HouseholdTaskServices
{
    public interface IHouseholdTaskService
    {

        public Task<HouseholdTaskDTO> AddTaskAsync(CreateHouseholdTaskRequest taskCreateRequest, UserClaims userClaims);
        public Task<IEnumerable<HouseholdTaskDTO>> GetallTasksAsync();

        public Task DeleteTaskByIdAsync(int taskId);

        public Task<HouseholdTaskDTO> GetByIdAsync(int taskId);

        public Task<HouseholdTaskDTO> UpdateTaskAsync(CreateHouseholdTaskRequest updateRequest, UserClaims userClaims, int id);

        public Task<IEnumerable<HouseholdTaskDTO>> GetallTasksByHouseholdIdAsync(UserClaims userClaims);
    }
}
