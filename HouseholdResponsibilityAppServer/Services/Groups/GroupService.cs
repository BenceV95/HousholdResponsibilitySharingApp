using HouseholdResponsibilityAppServer.Models;
using HouseholdResponsibilityAppServer.Models.Groups;
using HouseholdResponsibilityAppServer.Repositories.Groups;
using HouseholdResponsibilityAppServer.Repositories.HouseholdRepo;
using HouseholdResponsibilityAppServer.Services.HouseholdService;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace HouseholdResponsibilityAppServer.Services.Groups
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;

        // when creating a group we have to add  a household to it, so we need this dependecy to look up the household
        private readonly IHouseholdRepository _householdRepository;

        public GroupService(IGroupRepository groupRepository, IHouseholdRepository householdRepo)
        {
            _groupRepository = groupRepository;
            _householdRepository = householdRepo;
        }

        public async Task<IEnumerable<GroupResponseDto>> GetAllGroupsAsync()
        {
            var groups = await _groupRepository.GetAllGroupsAsync();

            return groups.Select(group => new GroupResponseDto
            {
                GroupResponseDtoId = group.GroupId,
                Name = group.Name,
            }).ToList();

        }

        public async Task<GroupResponseDto> GetGroupByIdAsync(int groupId)
        {
            var group = await _groupRepository.GetGroupByIdAsync(groupId);

            return new GroupResponseDto
            {
                GroupResponseDtoId = group.GroupId,
                Name = group.Name,
            };
        }

        public async Task CreateGroupAsync(PostGroupDto postGroupDto)
        {
            
            var household = await _householdRepository.GetHouseholdByIdAsync(postGroupDto.HouseholdId);

            var group = new TaskGroup
            {
                Name = postGroupDto.Name,
                Household = household
            };

            await _groupRepository.AddGroupAsync(group);
        }

        public async Task UpdateGroupAsync(int groupId, GroupDto groupDto)
        {
            var group = await _groupRepository.GetGroupByIdAsync(groupId);

            group.Name = groupDto.Name;

            await _groupRepository.UpdateGroupAsync(group);
        }

        public async Task DeleteGroupAsync(int groupId)
        {
            await _groupRepository.DeleteGroupAsync(groupId);
        }

    }
}
