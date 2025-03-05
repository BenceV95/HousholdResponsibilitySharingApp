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

        public async Task<IEnumerable<ScheduledTaskDTO>> GetAllScheduledByHouseholdIdAsync(UserClaims userClaims)
        {

            int householdId = int.Parse(userClaims.HouseholdId);

            var filteredTasks = await _scheduledTasksRepository.GetScheduledTasksByHouseholdIdAsync(householdId);

            return filteredTasks.Select(ConvertModelToDTO);
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
            var scheduledTaskModels = await _scheduledTasksRepository.GetAllScheduledTasksAsync();
            return scheduledTaskModels.Select(ConvertModelToDTO);
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
                throw new KeyNotFoundException("Household task not found!");
            }
            if (createdByUser == null)
            {
                throw new KeyNotFoundException("Created by user not found!");
            }
            if (assignedToUser == null)
            {
                throw new KeyNotFoundException("Assigned to user not found!");
            }

            // Checks whether the household associated with the task matches the createdByUser's household
            if (createdByUser.Household == null || createdByUser.Household.HouseholdId != task.Household.HouseholdId)
            {
                throw new UnauthorizedAccessException("You do not belong to the same household as the task!");
            }
            // Checks whether the assignedToUser is also in the same household
            if (assignedToUser.Household == null || assignedToUser.Household.HouseholdId != task.Household.HouseholdId)
            {
                throw new UnauthorizedAccessException("The user to assign the task to does not belong to the same household!");
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
            if (scheduledTaskModel == null)
            {
                throw new ArgumentNullException(nameof(scheduledTaskModel), "ScheduledTask model cannot be null");
            }

            return new ScheduledTaskDTO()
            {
                ScheduledTaskId = scheduledTaskModel.ScheduledTaskId,
                HouseholdTaskId = scheduledTaskModel.HouseholdTask?.TaskId ?? 0,
                CreatedByUserId = scheduledTaskModel.CreatedBy?.Id ?? string.Empty,
                AssignedToUserId = scheduledTaskModel.AssignedTo?.Id ?? string.Empty,
                EventDate = scheduledTaskModel.EventDate,
                CreatedAt = scheduledTaskModel.CreatedAt,
                AtSpecificTime = scheduledTaskModel.AtSpecificTime,
                Repeat = scheduledTaskModel.Repeat,
            };
        }

    }
}
