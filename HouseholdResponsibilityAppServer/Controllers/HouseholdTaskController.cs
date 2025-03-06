using HouseholdResponsibilityAppServer.Models.HouseholdTasks;
using HouseholdResponsibilityAppServer.Services.Authentication;
using HouseholdResponsibilityAppServer.Services.HouseholdTaskServices;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace HouseholdResponsibilityAppServer.Controllers
{
    [Authorize]
    [Route("/tasks")]
    [ApiController]
    public class HouseholdTaskController : ControllerBase
    {
        private readonly IHouseholdTaskService _householdTaskService;
        private readonly IAuthService _authService;
        private readonly ILogger<GroupController> _logger;

        public HouseholdTaskController(
            IHouseholdTaskService householdTaskService,
            IAuthService authService,
            ILogger<GroupController> logger)
        {
            _householdTaskService = householdTaskService;
            _authService = authService;
            _logger = logger;
        }

        [HttpGet("/tasks/my-household")]
        public async Task<IActionResult> GetHouseholdTasksByUsersHousehold()
        {
            var userClaims = _authService.GetClaimsFromHttpContext(HttpContext);
            try
            {
                var filteredTasks = await _householdTaskService.GetallTasksByHouseholdIdAsync(userClaims);

                return Ok(filteredTasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { Message = $"An error occurred while retrieving all tasks for household {userClaims.HouseholdId}." });
            }
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                var tasks = await _householdTaskService.GetallTasksAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { Message = $"An error occurred while retrieving all tasks." });
            }
        }

        [HttpGet("/task/{taskId}")]
        public async Task<IActionResult> GetTaskById(int taskId)
        {
            try
            {
                var task = await _householdTaskService.GetByIdAsync(taskId);
                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { Message = $"An error occurred while retrieving tasks with ID: {taskId}." });
            }
        }

        [HttpPost("/task")]
        public async Task<IActionResult> PostNewTask([FromBody] CreateHouseholdTaskRequest createRequest)
        {
            try
            {
                UserClaims userClaims = _authService.GetClaimsFromHttpContext(HttpContext);

                var task = await _householdTaskService.AddTaskAsync(createRequest, userClaims);
                return Ok(task.TaskId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { Message = $"An error occurred while posting task." });
            }

        }

        //dont forget to make dedicated class to patchrequest.
        [HttpPatch("/task/{taskId}")]
        public async Task<IActionResult> UpdateTask([FromBody] CreateHouseholdTaskRequest updateRequest, int taskId)
        {
            try
            {
                UserClaims userClaims = _authService.GetClaimsFromHttpContext(HttpContext);

                var task = await _householdTaskService.UpdateTaskAsync(updateRequest, userClaims, taskId);
                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { Message = $"An error occurred while updating task." });
            }
        }

        [HttpDelete("/task/{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            try
            {
                await _householdTaskService.DeleteTaskByIdAsync(taskId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { Message = $"An error occurred while deleting task with ID: {taskId}." });
            }
        }
    }
}
