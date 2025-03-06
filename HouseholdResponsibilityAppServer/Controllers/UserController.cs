using HouseholdResponsibilityAppServer.Models.Invitations;
using HouseholdResponsibilityAppServer.Models.Users;
using HouseholdResponsibilityAppServer.Services.Authentication;
using HouseholdResponsibilityAppServer.Services.Invitation;
using HouseholdResponsibilityAppServer.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IInvitationService _invitationService;
    private readonly IAuthService _authService;
    private readonly ILogger<GroupController> _logger;

    public UserController(
        IUserService userService,
        IInvitationService invitationService,
        IAuthService authService,
        ILogger<GroupController> logger)
    {
        _userService = userService;
        _invitationService = invitationService;
        _authService = authService;
        _logger = logger;
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
            _logger.LogError(ex.Message);

            return StatusCode(500, new { Message = "An error occurred while retrieving users." });
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
            _logger.LogError(ex.Message);

            return StatusCode(500, new { Message = "An error occurred while retrieving user." });
        }
    }

    [AllowAnonymous]
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
            _logger.LogError(ex.Message);

            return StatusCode(500, new { Message = "An error occurred while creating user." });
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
            _logger.LogError(ex.Message);

            return StatusCode(500, new { Message = "An error occurred while updating user." });
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
            _logger.LogError(ex.Message);

            return StatusCode(500, new { Message = "An error occurred while deleting user." });
        }
    }

    /// <summary>
    /// Gives back all the users in the same household (household id is from the token.)
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("/users/my-household")]
    public async Task<ActionResult> GetUsersByHouseholdId()
    {
        try
        {
            var userClaims = _authService.GetClaimsFromHttpContext(HttpContext);

            var filteredUsers = await _userService.GetAllUsersByHouseholdIdAsync(userClaims);

            return Ok(filteredUsers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return StatusCode(500, new { Message = "An error occurred while retrieving users." });

        }
    }

    /*
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
    */
}
