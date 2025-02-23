using HouseholdResponsibilityAppServer.Models.HouseholdTasks;
using HouseholdResponsibilityAppServer.Services.Authentication;
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
        private readonly IAuthService _authService;
        public HouseholdTaskController(IHouseholdTaskService householdTaskService, IAuthService authService)
        {
            _householdTaskService = householdTaskService;
            _authService = authService;
        }






        [HttpGet("filtered/{householdId}")]
        public async Task<IActionResult> GetHouseholdTasksByHouseholdId(int householdId)
        {
            try
            {
                var selectedTasks = await _householdTaskService.GetallTasksByHouseholdIdAsync(householdId);

                return Ok(selectedTasks);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                UserClaims userClaims = _authService.GetClaimsFromHttpContext(HttpContext);

                var task = await _householdTaskService.AddTaskAsync(createRequest, userClaims);
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
                UserClaims userClaims = _authService.GetClaimsFromHttpContext(HttpContext);

                var task = await _householdTaskService.UpdateTaskAsync(updateRequest, userClaims, taskId);
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
