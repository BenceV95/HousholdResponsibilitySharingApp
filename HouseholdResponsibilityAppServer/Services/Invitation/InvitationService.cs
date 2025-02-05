using HouseholdResponsibilityAppServer.Models;
using HouseholdResponsibilityAppServer.Models.Invitations;
using HouseholdResponsibilityAppServer.Repositories.InvitationRepo;
using HouseholdResponsibilityAppServer.Repositories.UserRepo;
using HouseholdResponsibilityAppServer.Services.Invitation;

public class InvitationService : IInvitationService
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly IUserRepository _userRepository;

    public InvitationService(IInvitationRepository invitationRepository, IUserRepository userRepository)
    {
        _invitationRepository = invitationRepository;
        _userRepository = userRepository;
    }

    public async Task InviteUserAsync(int householdId, InviteUserDto inviteUserDto)
    {
        var invitation = new Invitation
        {
            HouseholdId = householdId,
            Email = inviteUserDto.Email,
            InvitedBy = inviteUserDto.InvitedByUsername,
            CreatedAt = DateTime.UtcNow,
            IsAccepted = false
        };

        await _invitationRepository.CreateInvitationAsync(invitation);
    }

    public async Task AcceptInvitationAsync(int userId, AcceptInvitationDto acceptDto)
    {
        var invitation = await _invitationRepository.GetInvitationByEmailAsync(acceptDto.Email, acceptDto.HouseholdId);

        if (invitation == null || invitation.IsAccepted)
            throw new KeyNotFoundException("Invitation not found or already accepted");

        var user = await _userRepository.GetUserByIdAsync(userId);

        invitation.IsAccepted = true;

        await _invitationRepository.UpdateInvitationAsync(invitation);
        await _userRepository.UpdateUserAsync(user);
    }
}
