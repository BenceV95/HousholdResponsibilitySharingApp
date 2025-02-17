using HouseholdResponsibilityAppServer.Models.ScheduledTasks;
using HouseholdResponsibilityAppServer.Repositories.HouseholdTasks;
using HouseholdResponsibilityAppServer.Repositories.ScheduledTasks;
using HouseholdResponsibilityAppServer.Repositories.UserRepo;

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

        public async Task<ScheduledTaskDTO> AddScheduledTaskAsync(CreateScheduledTaskRequest scheduledTaskCreateRequest)
        {
            //convert request to modell
            var scheduledTaskModel = await ConvertRequestToModel(scheduledTaskCreateRequest);
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

        public async Task<ScheduledTaskDTO> UpdateScheduledTaskAsync(CreateScheduledTaskRequest updateRequest,int taskId)
        {
            var scheduledTaskModel = await ConvertRequestToModel(updateRequest);

            var updatedModel = await _scheduledTasksRepository.UpdateSheduledTaskAsync(scheduledTaskModel, taskId);
            return ConvertModelToDTO(updatedModel);
        }

        private async Task<ScheduledTask> ConvertRequestToModel(CreateScheduledTaskRequest scheduledTaskCreateRequest)
        {
            var task = await _householdTaskRepository.GetByIdAsync(scheduledTaskCreateRequest.HouseholdTaskId);
            var createdByUser = await _userRepository.GetUserByIdAsync(scheduledTaskCreateRequest.CreatedByUserId);
            var assignedToUser = await _userRepository.GetUserByIdAsync(scheduledTaskCreateRequest.AssignedToUserId);


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
    }
}
