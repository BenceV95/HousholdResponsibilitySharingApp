using HouseholdResponsibilityAppServer.Context;
using HouseholdResponsibilityAppServer.Models.Task;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using System.Runtime.CompilerServices;

namespace HouseholdResponsibilityAppServer.Repositories.HouseholdTasks
{
    public class HouseholdTasksRepository : IHouseholdTasksRepository
    {
        private HouseholdResponsibilityAppContext _dbContext;
        public HouseholdTasksRepository(HouseholdResponsibilityAppContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<HouseholdTask> AddTaskAsync(HouseholdTask householdTask)
        {
            await _dbContext.Tasks.AddAsync(householdTask);
            await _dbContext.SaveChangesAsync();
            return householdTask;
        }

        public async Task<IEnumerable<HouseholdTask>> GetAllTasksAsync()
        {
            return await _dbContext.Tasks
                .Include(t => t.CreatedBy) 
                .Include(t => t.Household) 
                .Include(t => t.Group) 
                .ToListAsync();
        }



        public async Task DeleteTaskByIdAsync(int taskId)
        {
            HouseholdTask householdTask = new() { TaskId = taskId };

            _dbContext.Entry(householdTask).State = EntityState.Deleted;

            await _dbContext.SaveChangesAsync();
        }


        public async Task<HouseholdTask> GetByIdAsync(int taskId)
        {
            var householdTask = await _dbContext.Tasks
                .Include(t => t.CreatedBy)
                .Include(t => t.Household)
                .Include(t => t.Group)
                .SingleOrDefaultAsync(t => t.TaskId == taskId);

            return householdTask ?? throw new KeyNotFoundException("No task was found with given Id!");
        }



        public async Task<HouseholdTask> UpdateTaskAsync(HouseholdTask householdTask, int id)
        {

            var existingTask = await _dbContext.Tasks.FindAsync(id);
            if (existingTask == null)
            {
                throw new KeyNotFoundException($"Couldn't find task in the db to update!");
            }

            existingTask.Title = householdTask.Title;
            existingTask.Description = householdTask.Description;
            existingTask.Priority = householdTask.Priority;
            existingTask.Group = householdTask.Group;

            //createdAt, createdBy shouldn't be updated I think

            await _dbContext.SaveChangesAsync();

            return existingTask;

        }


    }
}