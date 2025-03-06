using HouseholdResponsibilityAppServer.Models.Groups;
using HouseholdResponsibilityAppServer.Services.Authentication;
using HouseholdResponsibilityAppServer.Services.Groups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;
    private readonly IAuthService _authService;
    private readonly ILogger<GroupController> _logger;

    public GroupController(IGroupService groupService, IAuthService authService, ILogger<GroupController> logger)
    {
        _groupService = groupService;
        _authService = authService;
        _logger = logger;
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
            _logger.LogError(ex.Message);

            return StatusCode(500, new { Message = "An error occurred while retrieving all groups." });
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
        catch (KeyNotFoundException e)
        {
            return NotFound(new { message = e.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return StatusCode(500, new { Message = $"An error occurred while retrieving group with ID: {groupId}." });
        }
    }

    [HttpPost("/group")]
    public async Task<ActionResult> CreateGroup([FromBody] PostGroupDto postGroupDto)
    {
        try
        {
            UserClaims userClaims = _authService.GetClaimsFromHttpContext(HttpContext);

            await _groupService.CreateGroupAsync(postGroupDto, userClaims);
            return Ok(new { message = "Group created successfully" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return StatusCode(500, new { Message = "An error occurred while creating groups." });
        }
    }

    [HttpPut("/group/{groupId}")]
    public async Task<ActionResult> UpdateGroup(int groupId, [FromBody] GroupDto groupDto)
    {
        try
        {
            UserClaims userClaims = _authService.GetClaimsFromHttpContext(HttpContext);

            await _groupService.UpdateGroupAsync(groupId, groupDto, userClaims);
            return NoContent();
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { Message = e.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return StatusCode(500, new { Message = "An error occurred while updating group." });
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
            _logger.LogError(ex.Message);

            return StatusCode(500, new { Message = "An error occurred while deleting group." });
        }
    }


    [HttpGet("/groups/my-household")]
    public async Task<ActionResult> GetGroupsByHouseholdID()
    {
        try
        {
            var userClaims = _authService.GetClaimsFromHttpContext(HttpContext);

            var groups = await _groupService.GetGroupsByHouseholdIdAsync(userClaims);

            return Ok(groups);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return StatusCode(500, new { Message = "An error occurred while retrieving groups." });
        }
    }
}
