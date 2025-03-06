using HouseholdResponsibilityAppServer.Models.Histories;
using HouseholdResponsibilityAppServer.Models.HouseholdTasks;
using HouseholdResponsibilityAppServer.Services.HistoryServices;
using HouseholdResponsibilityAppServer.Services.HouseholdTaskServices;
using Microsoft.AspNetCore.Mvc;

namespace HouseholdResponsibilityAppServer.Controllers
{
    [Route("/histories")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        IHistoryService _historyService;
        public HistoryController(IHistoryService historyService)
        {
            _historyService = historyService;
        }


        [HttpGet()]
        public async Task<IActionResult> GetAllHistories()
        {
            try
            {
                var histories = await _historyService.GetallHistoriesAsync();
                return Ok(histories);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("/history/{historyId}")]
        public async Task<IActionResult> GetTaskById(int historyId)
        {
            try
            {
                var history = await _historyService.GetByIdAsync(historyId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("/history")]
        public async Task<IActionResult> PostNewHistory([FromBody] CreateHistoryRequest createRequest)
        {
            try
            {
                var history = await _historyService.AddHistoryAsync(createRequest);
                return Ok(history.HistoryId);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }


        [HttpPatch("/history/{historyId}")]
        public async Task<IActionResult> UpdateTask([FromBody] CreateHistoryRequest updateRequest)
        {
            try
            {
                var history = await _historyService.UpdateHistoryAsync(updateRequest);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
