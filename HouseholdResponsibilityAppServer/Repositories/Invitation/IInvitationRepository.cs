using HouseholdResponsibilityAppServer.Models.Invitations;

namespace HouseholdResponsibilityAppServer.Repositories.InvitationRepo
{
    public interface IInvitationRepository
    {
        Task CreateInvitationAsync(Invitation invitation);
        Task<Invitation> GetInvitationByEmailAsync(string email, int householdId);
        Task UpdateInvitationAsync(Invitation invitation);
    }

}
