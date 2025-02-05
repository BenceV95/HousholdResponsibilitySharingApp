using HouseholdResponsibilityAppServer.Models.HouseholdTasks;
using HouseholdResponsibilityAppServer.Models.ScheduledTask;
using HouseholdResponsibilityAppServer.Models.ScheduledTasks;
using HouseholdResponsibilityAppServer.Models.Task;
using HouseholdResponsibilityAppServer.Repositories.HouseholdTasks;
using HouseholdResponsibilityAppServer.Repositories.ScheduledTasks;
using System.Threading.Tasks;

namespace HouseholdResponsibilityAppServer.Services.ScheduledTaskServices
{
    public class ScheduledTaskService : IScheduledTaskService
    {
        private readonly IUserService _userService;
        private readonly IHouseholdTasksRepository _householdTaskRepository;
        private readonly IScheduledTasksRepository _scheduledTasksRepository;
        public ScheduledTaskService(IScheduledTasksRepository scheduledTasksRepository, IUserService userService, IHouseholdTasksRepository householdTaskRepository)
        {
            _userService = userService;
            _householdTaskRepository = householdTaskRepository;
            _scheduledTasksRepository = scheduledTasksRepository;
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

        public async Task<ScheduledTaskDTO> UpdateScheduledTaskAsync(CreateScheduledTaskRequest updateRequest)
        {
            var scheduledTaskModel = await ConvertRequestToModel(updateRequest);

            var updatedModel = await _scheduledTasksRepository.UpdateSheduledTaskAsync(scheduledTaskModel);
            return ConvertModelToDTO(updatedModel);
        }

        private async Task<ScheduledTask> ConvertRequestToModel(CreateScheduledTaskRequest scheduledTaskCreateRequest)
        {
            var task = await _householdTaskRepository.GetByIdAsync(scheduledTaskCreateRequest.HouseholdTaskId);
            var createdByUser = await _userService.GetUserByIdAsync(scheduledTaskCreateRequest.CreatedByUserId);
            var assignedToUser = await _userService.GetUserByIdAsync(scheduledTaskCreateRequest.AssignedToUserId);

            return new ScheduledTask()
            {
                CreatedBy = createdByUser,
                AssignedTo = assignedToUser,
                HouseholdTask = task,
                EventDate = scheduledTaskCreateRequest.EventDate,
                DayOfWeek = scheduledTaskCreateRequest.DayOfWeek,
                DayOfMonth = scheduledTaskCreateRequest.DayOfMonth,
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
                CreatedByUserId = scheduledTaskModel.CreatedBy.UserId,
                AssignedToUserId = scheduledTaskModel.AssignedTo.UserId,
                EventDate = scheduledTaskModel.EventDate,
                CreatedAt = scheduledTaskModel.CreatedAt,
                DayOfWeek = scheduledTaskModel.DayOfWeek,
                DayOfMonth = scheduledTaskModel.DayOfMonth,
                AtSpecificTime = scheduledTaskModel.AtSpecificTime,
                Repeat = scheduledTaskModel.Repeat,
            };
        }
    }
}
