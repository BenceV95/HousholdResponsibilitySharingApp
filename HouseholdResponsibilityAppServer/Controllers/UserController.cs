using HouseholdResponsibilityAppServer.DTOs;
using HouseholdResponsibilityAppServer.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    private readonly IInvitationService _invitationService;

    public UserController(IUserService userService, IInvitationService invitationService)
    {
        _userService = userService;
        _invitationService = invitationService;
    }

    [HttpGet("/users")]
    public async Task<ActionResult> GetAllUser()
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);

            return BadRequest("An error occurred while retrieving users.");
        }
    }

    [HttpGet("/user/{userId}")]
    public async Task<ActionResult> GetUserById(int userId)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(userId);
            return Ok(user);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);

            return BadRequest("An error occurred while retrieving user.");
        }
    }

    [HttpPost("/user")]
    public async Task<ActionResult> CreateUser([FromBody] UserDto userDto)
    {
        try
        {
            await _userService.CreateUserAsync(userDto);
            return Ok();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);

            return BadRequest("An error occurred while creating user.");
        }
    }

    [HttpPut("/user/{userId}")]
    public async Task<ActionResult> UpdateUser(int userId, [FromBody] UserDto userDto)
    {
        try
        {
            await _userService.UpdateUserAsync(userId, userDto);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);

            return BadRequest("An error occurred while updating user.");
        }

    }

    [HttpDelete("/user/{userId}")]
    public async Task<ActionResult> DeleteUser(int userId)
    {
        try
        {
            await _userService.DeleteUserAsync(userId);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);

            return NotFound("An error occurred while deleting user.");
        }
    }

    [HttpPost("/user/{userId}/accept-invitation")]
    public async Task<ActionResult> AcceptInvitation(int userId, [FromBody] AcceptInvitationDto acceptDto)
    {
        try
        {
            await _invitationService.AcceptInvitationAsync(userId, acceptDto);
            return Ok("Invitation accepted successfully");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            return BadRequest("An error occurred while accepting the invitation.");
        }
    }

}
