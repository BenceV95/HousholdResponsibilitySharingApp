using System.Diagnostics;
using System.Security.Claims;
using HouseholdResponsibilityAppServer.Models;
using HouseholdResponsibilityAppServer.Models.Households;
using HouseholdResponsibilityAppServer.Models.Invitations;
using HouseholdResponsibilityAppServer.Services.Authentication;
using HouseholdResponsibilityAppServer.Services.HouseholdService;
using HouseholdResponsibilityAppServer.Services.Invitation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

//[Authorize]
[ApiController]
public class HouseholdController : ControllerBase
{
    private readonly IHouseholdService _householdService;
    private readonly IInvitationService _invitationService;
    private readonly IAuthService _authService;
    private readonly ILogger<GroupController> _logger;

    public HouseholdController(
        IHouseholdService householdService,
        IInvitationService invitationService,
        IAuthService authService,
        ILogger<GroupController> logger)
    {
        _householdService = householdService;
        _invitationService = invitationService;
        _authService = authService;
        _logger = logger;
    }

    [HttpGet("/households")]
    public async Task<ActionResult> GetAllHousehold()
    {
        try
        {
            var households = await _householdService.GetAllHouseholdsAsync();
            return Ok(households);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return StatusCode(500, new { Message = "An error occurred while retrieving all Households." });
        }
    }

    [HttpGet("/household/{householdId}")]
    public async Task<ActionResult> GetHouseholdById(int householdId)
    {
        try
        {
            var household = await _householdService.GetHouseholdByIdAsync(householdId);
            return Ok(household);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new { Message = e.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return StatusCode(500, new { Message = "An error occurred while retrieving Household." });
        }
    }

    [HttpPost("/household")]
    public async Task<ActionResult> CreateHousehold([FromBody] HouseholdDto householdDto)
    {
        try
        {
            UserClaims userClaims = _authService.GetClaimsFromHttpContext(HttpContext);

            var createdHousehold = await _householdService.CreateHouseholdAsync(householdDto, userClaims);

            return Ok(createdHousehold.HouseholdId); // return the created household id
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, new { Message = "An error occurred while creating the Household." });
        }
    }

    [HttpPost("/household/join")]
    public async Task<ActionResult> JoinHousehold([FromQuery]int id)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _householdService.JoinHousehold(id, userId);

            return Ok(new {Message = $"User: {userId} has joined household: {id}."});
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new { Message = e.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, new { Message = $"An error occurred while joining Household with ID: {id}." });
        }
    }


    [HttpPut("/household/{householdId}")]
    public async Task<ActionResult> UpdateHousehold(int householdId, [FromBody] HouseholdDto householdDto)
    {
        try
        {
            await _householdService.UpdateHouseholdAsync(householdId, householdDto);
            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new { Message = e.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return StatusCode(500, new { Message = "An error occurred while updating the Household." });
        }
    }

    [HttpDelete("/household/{householdId}")]
    public async Task<ActionResult> DeleteHousehold(int householdId)
    {
        try
        {
            await _householdService.DeleteHouseholdAsync(householdId);
            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new { Message = e.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return StatusCode(500, new { Message = "An error occurred while deleting the Household." });
        }
    }

    // TODO: household invites to be implemented
    /*
    [HttpPost("/household/{householdId}/invite")]
    public async Task<ActionResult> InviteUserToHousehold(int householdId, [FromBody] InviteUserDto inviteUserDto)
    {
        try
        {
            await _invitationService.InviteUserAsync(householdId, inviteUserDto);
            return Ok("Invitation sent successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, new { Message = "An error occurred while creating the Invitation for Household." });
        }
    }
    */

}
