using HouseholdResponsibilityAppServer.Models.Invitations;
using HouseholdResponsibilityAppServer.Models.Users;
using HouseholdResponsibilityAppServer.Services.Invitation;
using HouseholdResponsibilityAppServer.Services.UserService;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
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
    public async Task<ActionResult> GetUserById(string userId)
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
            return Ok(new { message = "User created successfully!" }); //changed cuz frontend
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            return BadRequest(new { message = "An error occurred while creating user." });
        }
    }


    [HttpPut("/user/{userId}")]
    public async Task<ActionResult> UpdateUser(string userId, [FromBody] UserDto userDto)
    {
        try
        {
            await _userService.UpdateUserAsync(userId, userDto);
            return Ok(new { message = "Update successfull!" });  //changed cuz frontend
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);

            return BadRequest("An error occurred while updating user.");
        }
        
    }

    [HttpDelete("/user/{userId}")]
    public async Task<ActionResult> DeleteUser(string userId)
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
    public async Task<ActionResult> AcceptInvitation(string userId, [FromBody] AcceptInvitationDto acceptDto)
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
