using HouseholdResponsibilityAppServer.Models.Households;

namespace HouseholdResponsibilityAppServer.Repositories.HouseholdRepo
{
    public interface IHouseholdRepository
    {
        Task<IEnumerable<Household>> GetAllHouseholdsAsync();
        Task<Household> GetHouseholdByIdAsync(int id);
        Task AddHouseholdAsync(Household household);
        Task UpdateHouseholdAsync(Household household);
        Task DeleteHouseholdAsync(int id);

        Task InviteUserAsync(int householdId, string email, string invitedBy);
    }

}
