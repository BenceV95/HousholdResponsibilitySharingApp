using HouseholdResponsibilityAppServer.Models.ScheduledTasks;
using HouseholdResponsibilityAppServer.Services.Authentication;
using HouseholdResponsibilityAppServer.Services.ScheduledTaskServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseholdResponsibilityAppServer.Controllers
{
    [Authorize]
    [Route("/scheduleds")]
    [ApiController]
    public class ScheduledTaskController : ControllerBase
    {
        private readonly IScheduledTaskService _scheduledTaskService;
        private readonly IAuthService _authService;
        private readonly ILogger<GroupController> _logger;

        public ScheduledTaskController(
            IScheduledTaskService scheduledTaskService,
            IAuthService authService,
            ILogger<GroupController> logger)
        {
            _scheduledTaskService = scheduledTaskService;
            _authService = authService;
            _logger = logger;
        }


        [HttpGet()]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                var scheduledTasks = await _scheduledTaskService.GetAllScheduledTasksAsync();
                return Ok(scheduledTasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Message = "An error occurred while retrieving all Scheduled Tasks." });
            }
        }

        [HttpGet("/scheduled/{taskId}")]
        public async Task<IActionResult> GetTaskById(int taskId)
        {
            try
            {
                var task = await _scheduledTaskService.GetByIdAsync(taskId);
                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Message = "An error occurred while retrieving Scheduled Task." });
            }
        }

        [HttpPost("/scheduled")]
        public async Task<IActionResult> PostNewTask([FromBody] CreateScheduledTaskRequest createRequest)
        {
            try
            {
                var userClaims = _authService.GetClaimsFromHttpContext(HttpContext);

                var task = await _scheduledTaskService.AddScheduledTaskAsync(createRequest, userClaims);
                return Ok(task.ScheduledTaskId);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Message = "An error occurred while retrieving posting Scheduled Task." });
            }

        }

        //dont forget to make it work with the updaterequest.
        [HttpPatch("/scheduled/{taskId}")]
        public async Task<IActionResult> UpdateTask([FromBody] CreateScheduledTaskRequest updateRequest, int taskId)
        {
            try
            {
                var userClaims = _authService.GetClaimsFromHttpContext(HttpContext);

                var task = await _scheduledTaskService.UpdateScheduledTaskAsync(updateRequest, userClaims, taskId);
                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Message = "An error occurred while updating Scheduled Task." });
            }
        }

        [HttpDelete("/scheduled/{taskId}")]
        public async Task<IActionResult> DeleteScheduledTask(int taskId)
        {
            try
            {
                await _scheduledTaskService.DeleteScheduledTaskByIdAsync(taskId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Message = "An error occurred while deleting Scheduled Task." });
            }
        }


        /// <summary>
        /// This endpoint gives back all the scheduleds tasks, which belong to the same household
        /// </summary>
        /// <returns></returns>
        [HttpGet("/scheduleds/my-household")]
        public async Task<IActionResult> GetAllScheduledsByHousehold()
        {
            try
            {
                var userClaims = _authService.GetClaimsFromHttpContext(HttpContext);

                var filteredTasks = await _scheduledTaskService.GetAllScheduledByHouseholdIdAsync(userClaims);

                return Ok(filteredTasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Message = "An error occurred while retrieving all Scheduled Tasks." });
            }
        }

    }
}
