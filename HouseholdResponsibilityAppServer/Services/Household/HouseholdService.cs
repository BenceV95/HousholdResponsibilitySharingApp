using System.Diagnostics;
using HouseholdResponsibilityAppServer.Models;
using HouseholdResponsibilityAppServer.Models.Households;
using HouseholdResponsibilityAppServer.Models.Users;
using HouseholdResponsibilityAppServer.Repositories.HouseholdRepo;
using HouseholdResponsibilityAppServer.Repositories.UserRepo;
using HouseholdResponsibilityAppServer.Services.Authentication;
using Microsoft.AspNetCore.Identity;

namespace HouseholdResponsibilityAppServer.Services.HouseholdService
{
    public class HouseholdService : IHouseholdService
    {
        private readonly IHouseholdRepository _householdRepository;
        private readonly IUserRepository _userRepository;

        public HouseholdService(IHouseholdRepository householdRepository, IUserRepository userRepository)
        {
            _householdRepository = householdRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<HouseholdResponseDto>> GetAllHouseholdsAsync()
        {
            var households = await _householdRepository.GetAllHouseholdsAsync();

            return households.Select(household => new HouseholdResponseDto
            {
                HouseholdResponseDtoId = household.HouseholdId,
                Name = household.Name,
                CreatedAt = household.CreatedAt,
                CreatedByUsername = household.CreatedByUser.UserName
            }).ToList();
        }

        public async Task<HouseholdResponseDto> GetHouseholdByIdAsync(int householdId)
        {
            var household = await _householdRepository.GetHouseholdByIdAsync(householdId);

            return new HouseholdResponseDto
            {
                HouseholdResponseDtoId = household.HouseholdId,
                Name = household.Name,
                CreatedAt = household.CreatedAt,
                CreatedByUsername = household.CreatedByUser.UserName
            };
        }

        public async Task<Household> CreateHouseholdAsync(HouseholdDto householdDto, UserClaims userClaims)
        {
            // Check if the user already has a household
            var user = await _userRepository.GetUserByIdAsync(userClaims.UserId);
            if (user.Household != null)
            {
                throw new InvalidOperationException("User already has a household.");
            }
            
            var household = new Household
            {
                Name = householdDto.HouseholdName,
                CreatedByUser = user,
                CreatedAt = DateTime.UtcNow,
                Users = new List<User>(){user}
            };

            // first we assign the household to the user then we assign the admin user to the household
            var householdFromDB = await _householdRepository.AddHouseholdAsync(household);
            
            user.Household = householdFromDB;
            await _userRepository.UpdateUserAsync(user);

            return householdFromDB;
        }

        public async Task JoinHousehold(int id, string userId)
        {
            var household = await _householdRepository.GetHouseholdByIdAsync(id);
            
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null || household == null)
            {
                throw new KeyNotFoundException("User or household not found.");
            }

            // Check if the user is already part of the household
            if (household.Users.Any(u => u.Id == userId))
            {
                throw new InvalidOperationException("User is already a member of the household.");
            }
            household.Users.Add(user);
            user.Household = household;

            await _userRepository.UpdateUserAsync(user);
            await _householdRepository.UpdateHouseholdAsync(household);
        }

        public async Task UpdateHouseholdAsync(int householdId, HouseholdDto householdDto)
        {
            var household = await _householdRepository.GetHouseholdByIdAsync(householdId);

            household.Name = householdDto.HouseholdName;

            await _householdRepository.UpdateHouseholdAsync(household);
        }

        public async Task DeleteHouseholdAsync(int householdId)
        {
            await _householdRepository.DeleteHouseholdAsync(householdId);
        }

    }
}
