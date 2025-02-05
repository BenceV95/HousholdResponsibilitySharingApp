using HouseholdResponsibilityAppServer.DTOs;

namespace HouseholdResponsibilityAppServer.Services
{
    public interface IInvitationService
    {
        Task InviteUserAsync(int householdId, InviteUserDto inviteUserDto);
        Task AcceptInvitationAsync(int userId, AcceptInvitationDto acceptDto);
    }

}
