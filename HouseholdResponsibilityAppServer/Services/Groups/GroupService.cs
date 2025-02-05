using HouseholdResponsibilityAppServer.Models;
using HouseholdResponsibilityAppServer.Models.Groups;
using HouseholdResponsibilityAppServer.Repositories.Groups;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace HouseholdResponsibilityAppServer.Services.Groups
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;

        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
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

        public async Task CreateGroupAsync(GroupDto groupDto)
        {
            var group = new TaskGroup
            {
                Name = groupDto.Name,
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
