using HouseholdResponsibilityAppServer.Models;
using HouseholdResponsibilityAppServer.Models.Households;
using HouseholdResponsibilityAppServer.Services.Authentication;


namespace HouseholdResponsibilityAppServer.Services.HouseholdService
{
    public interface IHouseholdService
    {
        Task<IEnumerable<HouseholdResponseDto>> GetAllHouseholdsAsync();
        Task<HouseholdResponseDto> GetHouseholdByIdAsync(int id);
        public Task<Household> CreateHouseholdAsync(HouseholdDto householdDto, UserClaims userClaims);
        Task JoinHousehold(int id, string userId);
        Task UpdateHouseholdAsync(int id, HouseholdDto householdDto);
        Task DeleteHouseholdAsync(int id);
    }
}
