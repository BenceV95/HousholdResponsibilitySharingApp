using HouseholdResponsibilityAppServer.Models.HouseholdTasks;
using HouseholdResponsibilityAppServer.Models.ScheduledTasks;
using HouseholdResponsibilityAppServer.Services.HouseholdTaskServices;
using HouseholdResponsibilityAppServer.Services.ScheduledTaskServices;
using Microsoft.AspNetCore.Mvc;

namespace HouseholdResponsibilityAppServer.Controllers
{
    [Route("/scheduleds")]
    [ApiController]
    public class ScheduledTaskController : ControllerBase
    {
        IScheduledTaskService _scheduledTaskService;
        public ScheduledTaskController(IScheduledTaskService scheduledTaskService)
        {
            _scheduledTaskService = scheduledTaskService;
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
                var task = await _scheduledTaskService.AddScheduledTaskAsync(createRequest);
                return Ok(task.ScheduledTaskId);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPatch("/scheduled/{taskId}")]
        public async Task<IActionResult> UpdateTask([FromBody] CreateScheduledTaskRequest updateRequest)
        {
            try
            {
                var task = await _scheduledTaskService.UpdateScheduledTaskAsync(updateRequest);
                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
