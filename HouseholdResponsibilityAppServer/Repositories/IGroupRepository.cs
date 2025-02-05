using HouseholdResponsibilityAppServer.Models;

namespace HouseholdResponsibilityAppServer.Repositories
{
    public interface IGroupRepository
    {
        Task<IEnumerable<TaskGroup>> GetAllGroupsAsync();
        Task<TaskGroup> GetGroupByIdAsync(int id);
        Task AddGroupAsync(TaskGroup group);
        Task UpdateGroupAsync(TaskGroup group);
        Task DeleteGroupAsync(int id);
    }

}
