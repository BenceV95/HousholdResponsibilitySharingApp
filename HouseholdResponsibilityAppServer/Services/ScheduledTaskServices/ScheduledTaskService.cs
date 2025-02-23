using HouseholdResponsibilityAppServer.Models.ScheduledTasks;
using HouseholdResponsibilityAppServer.Repositories.HouseholdTasks;
using HouseholdResponsibilityAppServer.Repositories.ScheduledTasks;
using HouseholdResponsibilityAppServer.Repositories.UserRepo;
using HouseholdResponsibilityAppServer.Services.Authentication;

namespace HouseholdResponsibilityAppServer.Services.ScheduledTaskServices
{
    public class ScheduledTaskService : IScheduledTaskService
    {
        private readonly IHouseholdTasksRepository _householdTaskRepository;
        private readonly IScheduledTasksRepository _scheduledTasksRepository;
        private readonly IUserRepository _userRepository;
        public ScheduledTaskService(IScheduledTasksRepository scheduledTasksRepository, IHouseholdTasksRepository householdTaskRepository, IUserRepository userRepository)
        {
            _householdTaskRepository = householdTaskRepository;
            _scheduledTasksRepository = scheduledTasksRepository;
            _userRepository = userRepository;
        }

        public async Task<ScheduledTaskDTO> AddScheduledTaskAsync(CreateScheduledTaskRequest scheduledTaskCreateRequest, UserClaims userClaims)
        {
            //convert request to modell
            var scheduledTaskModel = await ConvertRequestToModel(scheduledTaskCreateRequest, userClaims);
            //add the modell to db
            var addedModel = await _scheduledTasksRepository.AddScheduledTaskAsync(scheduledTaskModel);
            //convert the added model to dto, and return it
            return ConvertModelToDTO(addedModel);
        }

        public async Task DeleteScheduledTaskByIdAsync(int scheduledTaskId)
        {
            await _scheduledTasksRepository.DeleteScheduledTaskByIdAsync(scheduledTaskId);
        }

        public async Task<IEnumerable<ScheduledTaskDTO>> GetAllScheduledTasksAsync()
        {
            List<ScheduledTaskDTO> scheduledTaskDTOs = new List<ScheduledTaskDTO>();
            var scheduledTaskModels = await _scheduledTasksRepository.GetAllScheduledTasksAsync();
            foreach (var task in scheduledTaskModels)
            {
                scheduledTaskDTOs.Add(ConvertModelToDTO(task));
            }
            return scheduledTaskDTOs;
        }

        public async Task<ScheduledTaskDTO> GetByIdAsync(int scheduledTaskId)
        {
            var scheduledTaskModel = await _scheduledTasksRepository.GetByIdAsync(scheduledTaskId);
            return ConvertModelToDTO(scheduledTaskModel);
        }

        public async Task<ScheduledTaskDTO> UpdateScheduledTaskAsync(CreateScheduledTaskRequest updateRequest, UserClaims userClaims, int taskId)
        {
            var scheduledTaskModel = await ConvertRequestToModel(updateRequest, userClaims);

            var updatedModel = await _scheduledTasksRepository.UpdateSheduledTaskAsync(scheduledTaskModel, taskId);
            return ConvertModelToDTO(updatedModel);
        }

        private async Task<ScheduledTask> ConvertRequestToModel(CreateScheduledTaskRequest scheduledTaskCreateRequest, UserClaims userClaims)
        {
            var task = await _householdTaskRepository.GetByIdAsync(scheduledTaskCreateRequest.HouseholdTaskId);
            var createdByUser = await _userRepository.GetUserByIdAsync(userClaims.UserId);
            var assignedToUser = await _userRepository.GetUserByIdAsync(scheduledTaskCreateRequest.AssignedToUserId);

            if (task == null)
            {
                throw new Exception("Household task not found!");
            }
            if (createdByUser == null)
            {
                throw new Exception("Created by user not found!");
            }
            if (assignedToUser == null)
            {
                throw new Exception("Assigned to user not found!");
            }

            // Checks whether the household associated with the task matches the createdByUser's household
            if (createdByUser.Household == null || createdByUser.Household.HouseholdId != task.Household.HouseholdId)
            {
                throw new Exception("You do not belong to the same household as the task!");
            }
            // Checks whether the assignedToUser is also in the same household
            if (assignedToUser.Household == null || assignedToUser.Household.HouseholdId != task.Household.HouseholdId)
            {
                throw new Exception("The user to assign the task to does not belong to the same household!");
            }


            return new ScheduledTask()
            {
                CreatedBy = createdByUser,
                AssignedTo = assignedToUser,
                HouseholdTask = task,
                EventDate = scheduledTaskCreateRequest.EventDate,
                AtSpecificTime = scheduledTaskCreateRequest.AtSpecificTime,
                Repeat = scheduledTaskCreateRequest.Repeat,

            };

        }
        private ScheduledTaskDTO ConvertModelToDTO(ScheduledTask scheduledTaskModel)
        {
            return new ScheduledTaskDTO()
            {
                ScheduledTaskId = scheduledTaskModel.ScheduledTaskId,
                HouseholdTaskId = scheduledTaskModel.HouseholdTask.TaskId,
                CreatedByUserId = scheduledTaskModel.CreatedBy.Id,
                AssignedToUserId = scheduledTaskModel.AssignedTo.Id,
                EventDate = scheduledTaskModel.EventDate,
                CreatedAt = scheduledTaskModel.CreatedAt,
                AtSpecificTime = scheduledTaskModel.AtSpecificTime,
                Repeat = scheduledTaskModel.Repeat,
            };
        }



        public async Task<IEnumerable<ScheduledTaskDTO>> GetAllScheduledByHouseholdIdAsync(UserClaims userClaims)
        {

            int householdId = int.Parse(userClaims.HouseholdId);
            
            var fillteredTasks = await _scheduledTasksRepository.GetScheduledTasksByHouseholdIdAsync(householdId);


            return fillteredTasks.Select(task => ConvertModelToDTO(task)).ToList();
        }


    }
}
