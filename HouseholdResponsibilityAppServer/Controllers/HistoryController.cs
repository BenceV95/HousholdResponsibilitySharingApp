using HouseholdResponsibilityAppServer.Models.Histories;
using HouseholdResponsibilityAppServer.Models.HouseholdTasks;
using HouseholdResponsibilityAppServer.Services.HistoryServices;
using HouseholdResponsibilityAppServer.Services.HouseholdTaskServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseholdResponsibilityAppServer.Controllers
{
    [Authorize]
    [Route("/histories")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;
        private readonly ILogger<GroupController> _logger;

        public HistoryController(IHistoryService historyService, ILogger<GroupController> logger)
        {
            _historyService = historyService;
            _logger = logger;
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
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Message = "An error occurred while retrieving all histories." });
            }
        }

        [HttpGet("/history/{historyId}")]
        public async Task<IActionResult> GetHistoryById(int historyId)
        {
            try
            {
                var history = await _historyService.GetByIdAsync(historyId);
                return Ok(history);
            }
            catch (KeyNotFoundException e)
            {
                return BadRequest(new { Message = e.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Message = "An error occurred while retrieving history." });
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
            catch (KeyNotFoundException e)
            {
                return BadRequest(new { Message = e.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Message = "An error occurred while posting history." });
            }

        }

        // we should make use of this history ID!
        [HttpPatch("/history/{historyId}")]
        public async Task<IActionResult> UpdateHistory([FromBody] CreateHistoryRequest updateRequest)
        {
            try
            {
                var history = await _historyService.UpdateHistoryAsync(updateRequest);
                return Ok(history);
            }
            catch (KeyNotFoundException e)
            {
                return BadRequest(new { Message = e.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Message = "An error occurred while updating history." });
            }
        }

        [HttpDelete("/history/{historyId}")]
        public async Task<IActionResult> DeleteHistory(int historyId)
        {
            try
            {
                await _historyService.DeleteHistoryByIdAsync(historyId);
                return NoContent();
            }
            catch (KeyNotFoundException e)
            {
                return BadRequest(new { Message = e.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Message = "An error occurred while deleting history." });
            }
        }
    }
}
