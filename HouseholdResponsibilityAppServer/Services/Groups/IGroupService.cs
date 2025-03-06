using HouseholdResponsibilityAppServer.Models;
using HouseholdResponsibilityAppServer.Models.Groups;
using HouseholdResponsibilityAppServer.Services.Authentication;


namespace HouseholdResponsibilityAppServer.Services.Groups
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupResponseDto>> GetAllGroupsAsync();
        Task<GroupResponseDto> GetGroupByIdAsync(int id);
        public Task CreateGroupAsync(PostGroupDto postGroupDto, UserClaims userClaims);
        Task UpdateGroupAsync(int groupId, GroupDto groupDto, UserClaims userClaims);
        Task DeleteGroupAsync(int groupId);
        public Task<IEnumerable<GroupResponseDto>> GetGroupsByHouseholdIdAsync(UserClaims userClaims);
    }
}
