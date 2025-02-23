using HouseholdResponsibilityAppServer.Models.HouseholdTasks;
using HouseholdResponsibilityAppServer.Models.ScheduledTasks;
using HouseholdResponsibilityAppServer.Services.Authentication;
using HouseholdResponsibilityAppServer.Services.HouseholdTaskServices;
using HouseholdResponsibilityAppServer.Services.ScheduledTaskServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace HouseholdResponsibilityAppServer.Controllers
{
    [Route("/scheduleds")]
    [ApiController]
    public class ScheduledTaskController : ControllerBase
    {
        IScheduledTaskService _scheduledTaskService;
        private readonly IAuthService _authService;
        public ScheduledTaskController(IScheduledTaskService scheduledTaskService, IAuthService authService)
        {
            _scheduledTaskService = scheduledTaskService;
            _authService = authService;
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
                return NotFound(ex.Message);
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
                return NotFound(ex.Message);
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
                return BadRequest(ex.Message);
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
                return BadRequest(ex.Message);
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
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// This endpoint gives back all the scheduleds tasks, which belong to the same household
        /// </summary>
        /// <returns></returns>
        [Authorize]
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
                return NotFound(ex.Message);
            }
        }

    }
}
