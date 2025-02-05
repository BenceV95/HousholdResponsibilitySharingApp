using HouseholdResponsibilityAppServer.DTOs;
using HouseholdResponsibilityAppServer.Models;


namespace HouseholdResponsibilityAppServer.Services
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
