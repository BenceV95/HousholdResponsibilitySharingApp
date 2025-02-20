using System.Diagnostics;
using System.Security.Claims;
using HouseholdResponsibilityAppServer.Models;
using HouseholdResponsibilityAppServer.Models.Households;
using HouseholdResponsibilityAppServer.Models.Invitations;
using HouseholdResponsibilityAppServer.Services.HouseholdService;
using HouseholdResponsibilityAppServer.Services.Invitation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class HouseholdController : ControllerBase
{
    private readonly IHouseholdService _householdService;

    private readonly IInvitationService _invitationService;

    public HouseholdController(IHouseholdService householdService, IInvitationService invitationService)
    {
        _householdService = householdService;
        _invitationService = invitationService;
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
            Console.Error.WriteLine(ex.Message);

            return BadRequest("An error occurred while retrieving households.");
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
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);

            return BadRequest("An error occurred while retrieving household.");
        }
    }

    [Authorize]
    [HttpPost("/household")]
    public async Task<ActionResult> CreateHousehold([FromBody] HouseholdDto householdDto)
    {
        try
        {
            // checking for if the user is the same as the one who is creating the household
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (householdDto.UserId != userId)
            {
                return Forbid();
            }

            var createdHousehold = await _householdService.CreateHouseholdAsync(householdDto);

            return Ok(createdHousehold.HouseholdId); // return the created household id
        }
        catch (Exception ex)
        {
            //Console.Error.WriteLine(ex.Message);
            
            return BadRequest("An error occurred while creating household.\n"+ex.Message);
        }
    }

    [Authorize]
    [HttpPost("/household/join")]
    public async Task<ActionResult> JoinHousehold([FromQuery]int id)
    {
        // this is a temp solution for the demo, validation need to be implemented !!!!
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Debug.WriteLine(id + " - " + userId);
            await _householdService.JoinHousehold(id, userId);

            return Ok(new {Message = $"User: {userId} has joined household: {id}."});
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            return BadRequest("An error occurred while joining the household.\n"+ex.Message);
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
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);

            return BadRequest("An error occurred while updating household.");
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
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);

            return NotFound("An error occurred while deleting household.");
        }
    }

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
            Console.Error.WriteLine(ex.Message);
            return BadRequest("An error occurred while sending the invitation.");
        }
    }

}
