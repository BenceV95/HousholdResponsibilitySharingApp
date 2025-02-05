using HouseholdResponsibilityAppServer.Models;

namespace HouseholdResponsibilityAppServer.Repositories
{
    public interface IInvitationRepository
    {
        Task CreateInvitationAsync(Invitation invitation);
        Task<Invitation> GetInvitationByEmailAsync(string email, int householdId);
        Task UpdateInvitationAsync(Invitation invitation);
    }

}
