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

            // get the household id form the claims (its a nullable string) so we have to parse it (if user is not in  a household, should be null, and throw an exception)
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
            var groups = await _groupRepository.GetAllGroupsAsync();


            return groups
                .Where(group => group.Household.HouseholdId == int.Parse(userClaims.HouseholdId))
                .Select(group => new GroupResponseDto
                {
                    GroupResponseDtoId = group.GroupId,
                    Name = group.Name,
                    HouseholdId = group.Household.HouseholdId
                }).ToList();
        }


    }
}
