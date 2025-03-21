using HouseholdResponsibilityAppServer.Context;
using HouseholdResponsibilityAppServer.Models.Invitations;
using Microsoft.EntityFrameworkCore;

namespace HouseholdResponsibilityAppServer.Repositories.InvitationRepo
{ 

public class InvitationRepository : IInvitationRepository
{
    private readonly HouseholdResponsibilityAppContext _context;

    public InvitationRepository(HouseholdResponsibilityAppContext context)
    {
        _context = context;
    }

    public async Task CreateInvitationAsync(Invitation invitation)
    {
        try
        {
            await _context.Invitations.AddAsync(invitation);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Database error: Unable to create invitation.");
        }
    }

    public async Task<Invitation> GetInvitationByEmailAsync(string email, int householdId)
    {
        try
        {
            return await _context.Invitations
            .FirstOrDefaultAsync(i => i.Email == email && i.HouseholdId == householdId && !i.IsAccepted);
        }
        catch (Exception ex)
        {
            throw new Exception("Database error: Unable to fetch invitation.");
        }
    }

    public async Task UpdateInvitationAsync(Invitation invitation)
    {
        _context.Invitations.Update(invitation);

        int affectedRows = await _context.SaveChangesAsync();

        if (affectedRows == 0) throw new KeyNotFoundException($"Invitation with ID {invitation.Id} not found.");
    }
}

}
