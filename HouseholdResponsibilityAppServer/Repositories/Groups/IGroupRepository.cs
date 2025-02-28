using HouseholdResponsibilityAppServer.Models.Groups;

namespace HouseholdResponsibilityAppServer.Repositories.Groups
{
    public interface IGroupRepository
    {
        Task<IEnumerable<TaskGroup>> GetAllGroupsAsync();
        Task<TaskGroup> GetGroupByIdAsync(int id);
        Task AddGroupAsync(TaskGroup group);
        Task UpdateGroupAsync(TaskGroup group);
        Task DeleteGroupAsync(int id);
        Task<IEnumerable<TaskGroup>> GetGroupsByHouseholdId(int householdId);
    }

}
