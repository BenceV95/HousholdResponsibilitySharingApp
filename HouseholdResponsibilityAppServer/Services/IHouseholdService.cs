using HouseholdResponsibilityAppServer.DTOs;
using HouseholdResponsibilityAppServer.Models;


namespace HouseholdResponsibilityAppServer.Services
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
