using HouseholdResponsibilityAppServer.Models.HouseholdTask;
using HouseholdResponsibilityAppServer.Models.Task;
using HouseholdResponsibilityAppServer.Repositories.HouseholdTasks;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.RegularExpressions;

namespace HouseholdResponsibilityAppServer.Services.HouseholdTaskServices
{
    public class HouseholdTaskService
    {
        private readonly IUserService _userService;
        private readonly IGroupService _groupService;
        private readonly IHouseholdTasksRepository _householdTaskRepository;
        public HouseholdTaskService(IUserService userService, IHouseholdTasksRepository householdTaskRepository, IHouseholdGroupService householdGroupService)
        {
            _userService = userService;
            _groupService = householdGroupService;
            _householdTaskRepository = householdTaskRepository;
        }

        public async Task<HouseholdTaskDTO> AddTaskAsync(CreateHouseholdTaskRequest taskCreateRequest)
        {
            //convert request to modell
            var taskModel = await ConvertRequestToModel(taskCreateRequest);
            //add the modell to db
            var addedModel = await _householdTaskRepository.AddTaskAsync(taskModel);
            //convert the added model to dto, and return it
            return ConvertModelToDTO(addedModel);
        }


        public async Task<IEnumerable<HouseholdTaskDTO>> GetallTasksAsync()
        {
            List<HouseholdTaskDTO> taskDTOs = new List<HouseholdTaskDTO>();
            var taskModels = await _householdTaskRepository.GetAllTasksAsync();
            foreach (var task in taskModels)
            {
                taskDTOs.Add(ConvertModelToDTO(task));
            }
            return taskDTOs;
        }

        public async Task DeleteTaskByIdAsync(int taskId)
        {
            //To consider: should i make the modell here, and pass that?
            await _householdTaskRepository.DeleteTaskByIdAsync(taskId);
        }
        public async Task<HouseholdTaskDTO> GetByIdAsync(int taskId)
        {
            var taskModel = await _householdTaskRepository.GetByIdAsync(taskId);
            return ConvertModelToDTO(taskModel);
        }

        // made with create request, no update request at the moment
        public async Task<HouseholdTaskDTO> UpdateTaskAsync(CreateHouseholdTaskRequest updateRequest)
        {
            var taskModel = await ConvertRequestToModel(updateRequest);

            var updatedModel = await _householdTaskRepository.UpdateTaskAsync(taskModel);
            return ConvertModelToDTO(updatedModel);
        }


        private async Task<HouseholdTask> ConvertRequestToModel(CreateHouseholdTaskRequest taskCreateRequest)
        {
            var group = await _groupService.GetGroupByIdAsync(taskCreateRequest.GroupId);
            var user = await _userService.GetUserByIdAsync(taskCreateRequest.UserId);

            return new HouseholdTask()
            {
                Title = taskCreateRequest.Title,
                Description = taskCreateRequest.Description,
                Priority = taskCreateRequest.Priority,
                Group = group,
                CreatedBy = user,
            };
        }

        private HouseholdTaskDTO ConvertModelToDTO(HouseholdTask taskModel)
        {
            return new HouseholdTaskDTO()
            {
                TaskId = taskModel.TaskId,
                Title = taskModel.Title,
                Description = taskModel.Description,
                UserId = taskModel.CreatedBy.Id,
                CreatedAt = taskModel.CreatedAt,
                GroupId = taskModel.Group.Id,
                Priority = taskModel.Priority,
            };
        }

    }
}
