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
            try
            {
                return await _context.Households.Include(h=>h.CreatedByUser).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Database error: Unable to fetch households.");
            }
        }

        public async Task<Household> GetHouseholdByIdAsync(int householdId)
        {
            try
            {
                var household = await _context.Households
                    .Include(h => h.CreatedByUser)
                    .FirstOrDefaultAsync(h => h.HouseholdId == householdId);

                if (household == null)
                {
                    throw new KeyNotFoundException($"Household with ID {householdId} not found.");
                }

                return household;
            }
            catch (Exception ex)
            {
                throw new Exception("Database error: Unable to fetch household by ID.");
            }
        }

        public async Task<Household> AddHouseholdAsync(Household household)
        {
            try
            {
                await _context.Households.AddAsync(household);
                await _context.SaveChangesAsync();
                return household; // should return it with the ID
            }
            catch (Exception ex)
            {
                throw new Exception("Database error: Unable to create household.");
            }
        }

        public async Task UpdateHouseholdAsync(Household household)
        {
            _context.Households.Update(household);

            int affectedRows = await _context.SaveChangesAsync();

            if (affectedRows == 0) throw new KeyNotFoundException($"User with ID {household.HouseholdId} not found.");
        }

        /*public async Task DeleteHouseholdAsync(int householdId)
        {
            var household = new Household { HouseholdId = householdId };

            _context.Entry(household).State = EntityState.Deleted;

            int affectedRows = await _context.SaveChangesAsync();

            if (affectedRows == 0) throw new KeyNotFoundException($"Household with ID {householdId} not found.");
        }
        */

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
            try
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
            catch (Exception ex)
            {
                throw new Exception("An error occurred while inviting the user.");
            }
        }

    }

}
