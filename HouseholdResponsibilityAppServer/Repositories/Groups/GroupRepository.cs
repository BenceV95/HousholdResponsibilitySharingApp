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
            try
            {
                return await _context.Groups.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Database error: Unable to fetch groups.");
            }
        }

        public async Task<TaskGroup> GetGroupByIdAsync(int groupId)
        {
            try
            {
                var group = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == groupId);

                if (group == null)
                {
                    throw new KeyNotFoundException($"Group with ID {groupId} not found.");
                }

                return group;
            }
            catch (Exception ex)
            {
                throw new Exception("Database error: Unable to fetch group by ID.");
            }
        }

        public async Task AddGroupAsync(TaskGroup group)
        {
            try
            {
                await _context.Groups.AddAsync(group);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Database error: Unable to create group.");
            }
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
    }

}
