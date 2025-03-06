using HouseholdResponsibilityAppServer.Context;
using HouseholdResponsibilityAppServer.Models.Groups;
using Microsoft.EntityFrameworkCore;

namespace HouseholdResponsibilityAppServer.Repositories.Groups
{
    public class GroupRepository : IGroupRepository
    {
        private readonly HouseholdResponsibilityAppContext _context;

        public GroupRepository(HouseholdResponsibilityAppContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskGroup>> GetAllGroupsAsync()
        {
            return await _context.Groups.Include(g => g.Household).ToListAsync();
        }

        public async Task<TaskGroup> GetGroupByIdAsync(int groupId)
        {
            var group = await _context.Groups
                .Include(g => g.Household)
                .FirstOrDefaultAsync(g => g.GroupId == groupId);

            if (group == null)
            {
                throw new KeyNotFoundException($"Group with ID {groupId} not found.");
            }

            return group;
        }

        public async Task AddGroupAsync(TaskGroup group)
        {
            await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateGroupAsync(TaskGroup group)
        {
            _context.Groups.Update(group);

            int affectedRows = await _context.SaveChangesAsync();

            if (affectedRows == 0) throw new KeyNotFoundException($"Group with ID {group.GroupId} not found.");
        }

        /*public async Task DeleteGroupAsync(int groupId)
        {
            var group = new TaskGroup { GroupId = groupId };

            _context.Entry(group).State = EntityState.Deleted;

            int affectedRows = await _context.SaveChangesAsync();

            if (affectedRows == 0) throw new KeyNotFoundException($"Group with ID {groupId} not found.");

        }
        */

        public async Task DeleteGroupAsync(int groupId)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == groupId);

            if (group == null)
            {
                throw new KeyNotFoundException($"Group with ID {groupId} not found.");
            }

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaskGroup>> GetGroupsByHouseholdId(int householdId)
        {
            return await _context.Groups
                .Include(g => g.Household)
                .Where(g => g.Household.HouseholdId == householdId)
                .ToListAsync();
        }
    }

}
