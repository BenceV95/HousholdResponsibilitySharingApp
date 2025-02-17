using HouseholdResponsibilityAppServer.Models;
using HouseholdResponsibilityAppServer.Models.Households;
using HouseholdResponsibilityAppServer.Repositories.HouseholdRepo;
using Microsoft.AspNetCore.Identity;

namespace HouseholdResponsibilityAppServer.Services.HouseholdService
{
    public class HouseholdService : IHouseholdService
    {
        private readonly IHouseholdRepository _householdRepository;

        public HouseholdService(IHouseholdRepository householdRepository)
        {
            _householdRepository = householdRepository;
        }

        public async Task<IEnumerable<HouseholdResponseDto>> GetAllHouseholdsAsync()
        {
            var households = await _householdRepository.GetAllHouseholdsAsync();

            return households.Select(household => new HouseholdResponseDto
            {
                HouseholdResponseDtoId = household.HouseholdId,
                Name = household.Name,
                CreatedAt = household.CreatedAt,
                CreatedByUsername = household.CreatedByUser?.UserName ?? "Unknown"
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
                CreatedByUsername = household.CreatedByUser?.UserName ?? "Unknown"
            };
        }

        public async Task CreateHouseholdAsync(HouseholdDto householdDto)
        {
            var household = new Household
            {
                Name = householdDto.Name,
                CreatedAt = DateTime.UtcNow,
            };

            await _householdRepository.AddHouseholdAsync(household);
        }

        public async Task UpdateHouseholdAsync(int householdId, HouseholdDto householdDto)
        {
            var household = await _householdRepository.GetHouseholdByIdAsync(householdId);

            household.Name = householdDto.Name;

            await _householdRepository.UpdateHouseholdAsync(household);
        }

        public async Task DeleteHouseholdAsync(int householdId)
        {
            await _householdRepository.DeleteHouseholdAsync(householdId);
        }

    }
}
