using HouseholdResponsibilityAppServer.Models.Households;
using HouseholdResponsibilityAppServer.Models.Task;

namespace HouseholdResponsibilityAppServer.Repositories.HouseholdTasks
{
    public interface IHouseholdTasksRepository
    {
        public Task<HouseholdTask> AddTaskAsync(HouseholdTask householdTask);
        public Task<IEnumerable<HouseholdTask>> GetAllTasksAsync();
        public Task DeleteTaskByIdAsync(int taskId);
        public Task<HouseholdTask> GetByIdAsync(int taskId);
        public Task<HouseholdTask> UpdateTaskAsync(HouseholdTask householdTask, int id);

        public Task<IEnumerable<HouseholdTask>> GetallTasksByHouseholdIdAsync(int householdId);
    }
}
