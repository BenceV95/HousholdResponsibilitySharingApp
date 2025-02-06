using HouseholdResponsibilityAppServer.Models;
using HouseholdResponsibilityAppServer.Models.Households;


namespace HouseholdResponsibilityAppServer.Services.HouseholdService
{
    public interface IHouseholdService
    {
        Task<IEnumerable<HouseholdResponseDto>> GetAllHouseholdsAsync();
        Task<HouseholdResponseDto> GetHouseholdByIdAsync(int id);
        Task CreateHouseholdAsync(HouseholdDto householdDto);
        Task UpdateHouseholdAsync(int id, HouseholdDto householdDto);
        Task DeleteHouseholdAsync(int id);
    }
}
