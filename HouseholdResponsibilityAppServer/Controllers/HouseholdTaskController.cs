using HouseholdResponsibilityAppServer.Models.HouseholdTasks;
using HouseholdResponsibilityAppServer.Services.HouseholdTaskServices;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HouseholdResponsibilityAppServer.Controllers
{
    [Route("/tasks")]
    [ApiController]
    public class HouseholdTaskController : ControllerBase
    {
        IHouseholdTaskService _householdTaskService;
        public HouseholdTaskController(IHouseholdTaskService householdTaskService)
        {
            _householdTaskService = householdTaskService;
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
                return NotFound(ex.Message);
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
                return NotFound(ex.Message);
            }
        }

        [HttpPost("/task")]
        public async Task<IActionResult> PostNewTask([FromBody] CreateHouseholdTaskRequest createRequest)
        {
            try
            {
                var task = await _householdTaskService.AddTaskAsync(createRequest);
                return Ok(task.TaskId);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        //dont forget to make dedicated class to patchrequest.
        [HttpPatch("/task/{taskId}")]
        public async Task<IActionResult> UpdateTask([FromBody] CreateHouseholdTaskRequest updateRequest, int taskId)
        {
            try
            {
                var task = await _householdTaskService.UpdateTaskAsync(updateRequest, taskId);
                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                return BadRequest(ex.Message);
            }
        }


    }
}
