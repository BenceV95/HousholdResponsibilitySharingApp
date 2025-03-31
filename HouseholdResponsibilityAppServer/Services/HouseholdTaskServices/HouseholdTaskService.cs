using HouseholdResponsibilityAppServer.Models.HouseholdTasks;
using HouseholdResponsibilityAppServer.Models.Task;
using HouseholdResponsibilityAppServer.Repositories.Groups;
using HouseholdResponsibilityAppServer.Repositories.HouseholdRepo;
using HouseholdResponsibilityAppServer.Repositories.HouseholdTasks;
using HouseholdResponsibilityAppServer.Repositories.UserRepo;
using HouseholdResponsibilityAppServer.Services.Authentication;


namespace HouseholdResponsibilityAppServer.Services.HouseholdTaskServices
{
    public class HouseholdTaskService : IHouseholdTaskService
    {
        private readonly IHouseholdTasksRepository _householdTaskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IHouseholdRepository _householdRepository;


        public HouseholdTaskService(
            IHouseholdTasksRepository householdTaskRepository,
            IUserRepository userRepository,
            IGroupRepository groupRepository,
            IHouseholdRepository householdRepository)
        {
            _householdTaskRepository = householdTaskRepository;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _householdRepository = householdRepository;
        }

        public async Task<IEnumerable<HouseholdTaskDTO>> GetallTasksAsync()
        {
            var taskModels = await _householdTaskRepository.GetAllTasksAsync();
            return taskModels.Select(ConvertModelToDTO);
        }

        public async Task<HouseholdTaskDTO> GetByIdAsync(int taskId)
        {
            var taskModel = await _householdTaskRepository.GetByIdAsync(taskId);
            return ConvertModelToDTO(taskModel);
        }

        public async Task<IEnumerable<HouseholdTaskDTO>> GetallTasksByHouseholdIdAsync(UserClaims userClaims)
        {
            int householdId = int.Parse(userClaims.HouseholdId);

            var filteredTasks = await _householdTaskRepository.GetAllTasksByHouseholdIdAsync(householdId);

            return filteredTasks.Select(ConvertModelToDTO);
        }

        public async Task<HouseholdTaskDTO> AddTaskAsync(CreateHouseholdTaskRequest taskCreateRequest, UserClaims userClaims)
        {
            //convert request to modell
            var taskModel = await ConvertRequestToModel(taskCreateRequest, userClaims);
            //add the modell to db
            var addedModel = await _householdTaskRepository.AddTaskAsync(taskModel);
            //convert the added model to dto, and return it
            return ConvertModelToDTO(addedModel);
        }

        // made with create request, no update request at the moment
        public async Task<HouseholdTaskDTO> UpdateTaskAsync(CreateHouseholdTaskRequest updateRequest, UserClaims userClaims, int id)
        {
            var taskModel = await ConvertRequestToModel(updateRequest, userClaims);

            var updatedModel = await _householdTaskRepository.UpdateTaskAsync(taskModel, id);
            return ConvertModelToDTO(updatedModel);
        }

        public async Task DeleteTaskByIdAsync(int taskId)
        {
            await _householdTaskRepository.DeleteTaskByIdAsync(taskId);
        }


        private async Task<HouseholdTask> ConvertRequestToModel(CreateHouseholdTaskRequest taskCreateRequest, UserClaims userClaims)
        {
            var group = await _groupRepository.GetGroupByIdAsync(taskCreateRequest.GroupId);
            var user = await _userRepository.GetUserByIdAsync(userClaims.UserId);
            var household = await _householdRepository.GetHouseholdByIdAsync(int.Parse(userClaims.HouseholdId));

            if (user == null)
            {
                throw new KeyNotFoundException("User not found!");
            }

            if (household == null)
            {
                throw new KeyNotFoundException("Household not found!");
            }

            // Checks whether the user belongs to the correct household
            if (user.Household == null || user.Household.HouseholdId != household.HouseholdId)
            {
                throw new UnauthorizedAccessException("You do not belong to this household!");
            }

            // Checks whether the selected group belongs to the specified household
            if (group.Household == null || group.Household.HouseholdId != household.HouseholdId)
            {
                throw new UnauthorizedAccessException("The selected group does not belong to your household!");
            }


            return new HouseholdTask()
            {
                Title = taskCreateRequest.Title,
                Description = taskCreateRequest.Description,
                Priority = taskCreateRequest.Priority,
                Group = group,
                CreatedBy = user,
                Household = household
            };
        }

        private HouseholdTaskDTO ConvertModelToDTO(HouseholdTask taskModel)
        {
            return new HouseholdTaskDTO()
            {
                TaskId = taskModel.TaskId,
                Title = taskModel.Title,
                Description = taskModel.Description,
                UserId = taskModel.CreatedBy?.Id ?? string.Empty,
                CreatedAt = taskModel.CreatedAt,
                GroupId = taskModel.Group?.GroupId ?? 0,
                Priority = taskModel.Priority,
                HouseholdId = taskModel.Household?.HouseholdId ?? 0
            };
        }

    }
}