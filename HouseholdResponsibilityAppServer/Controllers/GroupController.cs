using HouseholdResponsibilityAppServer.Models.Groups;
using HouseholdResponsibilityAppServer.Services.Groups;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpGet("/groups")]
    public async Task<ActionResult> GetAllGroup()
    {
        try
        {
            var groups = await _groupService.GetAllGroupsAsync();
            return Ok(groups);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);

            return BadRequest("An error occurred while retrieving groups.");
        }
    }

    [HttpGet("/group/{groupId}")]
    public async Task<ActionResult> GetGroupById(int groupId)
    {
        try
        {
            var group = await _groupService.GetGroupByIdAsync(groupId);
            return Ok(group);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);

            return BadRequest("An error occurred while retrieving groups.");
        }
    }

    [HttpPost("/group")]
    public async Task<ActionResult> CreateGroup([FromBody] GroupDto groupDto)
    {
        try
        {
            await _groupService.CreateGroupAsync(groupDto);
            return Ok();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);

            return BadRequest("An error occurred while creating group.");
        }
    }

    [HttpPut("/group/{groupId}")]
    public async Task<ActionResult> UpdateGroup(int groupId, [FromBody] GroupDto groupDto)
    {
        try
        {
            await _groupService.UpdateGroupAsync(groupId, groupDto);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);

            return BadRequest("An error occurred while updating group.");
        }
    }

    [HttpDelete("/group/{groupId}")]
    public async Task<ActionResult> DeleteGroup(int groupId)
    {
        try
        {
            await _groupService.DeleteGroupAsync(groupId);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);

            return NotFound("An error occurred while deleting group.");
        }
    }
}
