using HouseholdResponsibilityAppServer.Context;
using HouseholdResponsibilityAppServer.Models;
using HouseholdResponsibilityAppServer.Models.Groups;
using HouseholdResponsibilityAppServer.Models.Households;
using HouseholdResponsibilityAppServer.Repositories.Groups;
using HouseholdResponsibilityAppServer.Repositories.HouseholdRepo;
using HouseholdResponsibilityAppServer.Services.Authentication;
using HouseholdResponsibilityAppServer.Services.HouseholdService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace HouseholdResponsibilityAppServer.Services.Groups
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;


        private readonly IHouseholdRepository _householdRepository;


        public GroupService(IGroupRepository groupRepository, IHouseholdRepository householdRepository)
        {
            _groupRepository = groupRepository;
            _householdRepository = householdRepository;

        }
        public async Task<IEnumerable<GroupResponseDto>> GetAllGroupsAsync()
        {
            var groups = await _groupRepository.GetAllGroupsAsync();

            return groups.Select(group => new GroupResponseDto
            {
                GroupResponseDtoId = group.GroupId,
                Name = group.Name,
                HouseholdId = group.Household.HouseholdId
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

        public async Task CreateGroupAsync(PostGroupDto postGroupDto, UserClaims userClaims)
        {
            // get the household id form the claims (it's a nullable string) so we have to parse it (if user is not in  a household, should be null, and throw an exception)
            var isNumber = int.TryParse(userClaims.HouseholdId, out int householdId);

            if (!isNumber)
            {
                throw new ArgumentException("Cannot create group, user is not in a household!");
            }

            var household = await _householdRepository.GetHouseholdByIdAsync(householdId);

            var group = new TaskGroup
            {
                Name = postGroupDto.GroupName,
                Household = household,

            };

            await _groupRepository.AddGroupAsync(group);
        }

        public async Task UpdateGroupAsync(int groupId, GroupDto groupDto)
        {
            if (groupDto.Name.Length < 1 || groupDto.Name == " ")
            {
                throw new ArgumentException("Name can not be empty !");
            }

            var group = await _groupRepository.GetGroupByIdAsync(groupId);

            group.Name = groupDto.Name;

            await _groupRepository.UpdateGroupAsync(group);
        }

        public async Task DeleteGroupAsync(int groupId)
        {
            await _groupRepository.DeleteGroupAsync(groupId);
        }

        public async Task<IEnumerable<GroupResponseDto>> GetGroupsByHouseholdIdAsync(UserClaims userClaims)
        {
            if (string.IsNullOrEmpty(userClaims.HouseholdId))
            {
                throw new ArgumentException($"{userClaims.UserName} is not part of a household.");
            }

            var householdId = int.Parse(userClaims.HouseholdId);

            var filteredGroups = await _groupRepository.GetGroupsByHouseholdId(householdId);

            return filteredGroups.Select(group => ConvertModelToDto(group));
        }

        private GroupResponseDto ConvertModelToDto(TaskGroup taskGroup)
        {
            return new GroupResponseDto()
            {
                //for some reason, this is the task group's id
                GroupResponseDtoId = taskGroup.GroupId,
                Name = taskGroup.Name,
                HouseholdId = taskGroup.Household.HouseholdId
            };
        }

    }
}
