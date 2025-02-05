using HouseholdResponsibilityAppServer.Models;
using HouseholdResponsibilityAppServer.Models.Groups;


namespace HouseholdResponsibilityAppServer.Services.Groups
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupResponseDto>> GetAllGroupsAsync();
        Task<GroupResponseDto> GetGroupByIdAsync(int id);
        Task CreateGroupAsync(GroupDto groupDto);
        Task UpdateGroupAsync(int id, GroupDto groupDto);
        Task DeleteGroupAsync(int id);
    }
}
