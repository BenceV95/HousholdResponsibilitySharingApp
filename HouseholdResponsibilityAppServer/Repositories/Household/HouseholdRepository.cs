using HouseholdResponsibilityAppServer.Context;
using HouseholdResponsibilityAppServer.Models.Households;
using HouseholdResponsibilityAppServer.Models.Invitations;
using Microsoft.EntityFrameworkCore;

namespace HouseholdResponsibilityAppServer.Repositories.HouseholdRepo
{
    public class HouseholdRepository : IHouseholdRepository
    {
        private readonly HouseholdResponsibilityAppContext _context;

        public HouseholdRepository(HouseholdResponsibilityAppContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Household>> GetAllHouseholdsAsync()
        {
            return await _context.Households
                .Include(h => h.CreatedByUser)
                .Include(u => u.Users)
                .ToListAsync();
        }

        public async Task<Household> GetHouseholdByIdAsync(int householdId)
        {
            var household = await _context.Households
                .Include(h => h.CreatedByUser)
                .Include(u => u.Users)
                .FirstOrDefaultAsync(h => h.HouseholdId == householdId);

            if (household == null)
            {
                throw new KeyNotFoundException($"Household with ID {householdId} not found.");
            }

            household.Users = household.Users.ToList();
            return household;
        }

        public async Task<Household> AddHouseholdAsync(Household household)
        {

            await _context.Households.AddAsync(household);
            await _context.SaveChangesAsync();
            return household; // should return it with the ID

        }

        public async Task UpdateHouseholdAsync(Household household)
        {
            _context.Households.Update(household);

            int affectedRows = await _context.SaveChangesAsync();

            if (affectedRows == 0) throw new KeyNotFoundException($"Household with ID {household.HouseholdId} not found.");
        }

        public async Task DeleteHouseholdAsync(int householdId)
        {
            var household = await _context.Households.FirstOrDefaultAsync(h => h.HouseholdId == householdId);

            if (household == null)
            {
                throw new KeyNotFoundException($"Household with ID {householdId} not found.");
            }

            _context.Households.Remove(household);
            await _context.SaveChangesAsync();
        }

        public async Task InviteUserAsync(int householdId, string email, string invitedBy)
        {

            var household = await _context.Households.FirstOrDefaultAsync(h => h.HouseholdId == householdId);

            if (household == null) throw new KeyNotFoundException("Household not found");

            var invitation = new Invitation
            {
                HouseholdId = householdId,
                Email = email,
                InvitedBy = invitedBy,
                CreatedAt = DateTime.UtcNow,
                IsAccepted = false // Még nincs elfogadva
            };

            await _context.Invitations.AddAsync(invitation);
            await _context.SaveChangesAsync();
        }
    }
}
