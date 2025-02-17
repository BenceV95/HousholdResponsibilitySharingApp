using HouseholdResponsibilityAppServer.Models.Invitations;

namespace HouseholdResponsibilityAppServer.Services.Invitation
{
    public interface IInvitationService
    {
        Task InviteUserAsync(int householdId, InviteUserDto inviteUserDto);
        Task AcceptInvitationAsync(string userId, AcceptInvitationDto acceptDto);
    }

}
