using HouseholdResponsibilityAppServer.Contracts;
using HouseholdResponsibilityAppServer.Repositories.UserRepo;
using HouseholdResponsibilityAppServer.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace HouseholdResponsibilityAppServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authenticationService;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        const int _expirationTimeInMinutes = 30;

        public AuthController(IAuthService authenticationService, IUserRepository userRepository, ITokenService tokenService)
        {
            _authenticationService = authenticationService;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _authenticationService.RegisterAsync(request, "User");

            if (!result.Success)
            {
                AddErrors(result);
                return BadRequest(ModelState);
            }

            return CreatedAtAction(nameof(Register), new RegistrationResponse(result.Email, result.UserName));
        }

        private void AddErrors(AuthResult result)
        {
            foreach (var error in result.ErrorMessages)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authenticationService.LoginAsync(request.Email, request.Password);

            if (!result.Success)
            {
                foreach (var error in result.ErrorMessages)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
                return BadRequest(ModelState);
            }


            Response.Cookies.Append("token", result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddMinutes(_expirationTimeInMinutes)
            });

            //here we should just send back meta data that we want to display on the front end (user Id we dont need, cause on subsequent request we get it from the token);
            return Ok(new AuthResponse(result.Email, result.UserName, result.HouseholdId));
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("token");
            return Ok(new { message = "Logout successful" });
        }



        [Authorize]
        [HttpGet("/authenticate")]
        public IActionResult Me()
        {
            var user = HttpContext.User;
            if (user == null)
                return Unauthorized();

            var email = user.FindFirst(ClaimTypes.Email)?.Value;
            var username = user.FindFirst(ClaimTypes.Name)?.Value;
            var expClaim = user.FindFirst("exp")?.Value;
            long expirationUnix = 0;

            if (expClaim != null)
            {
                long.TryParse(expClaim, out expirationUnix);
            }
            return Ok(new { email, username, expirationUnix });
        }

        [Authorize]
        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "User"; // just a fallback if we cannot get the user's role
            if (userId == null) return Unauthorized();

            // Fetch updated user details (now including HouseholdId)
            var user = await _userRepository.GetUserByIdAsync(userId);


            if (user == null)
            {
                return Unauthorized();
            }


            Response.Cookies.Delete("token"); // Ensure old token is removed before adding the new one



            // Generate new token with updated HouseholdId
            var token = _tokenService.CreateToken(user, userRole);


            Response.Cookies.Append("token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddMinutes(_expirationTimeInMinutes)
            });

            return Ok(new { message = "Token refreshed successfully" });
        }


    }
}



//try
//{
//    if (UserHelper.GetUserId(HttpContext, out var userId, out var unauthorized))
//        return unauthorized;

//    Console.WriteLine(userId);

//    var accounts = await _accountService.GetAll(userId);
//    return Ok(accounts);
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message);
//    return BadRequest(ex.Message);
//}




//using System.Security.Claims;
//using Microsoft.AspNetCore.Mvc;

//namespace Aurum.Utils;

//public static class UserHelper
//{
//    public static bool GetUserId(HttpContext context, out string? userId, out IActionResult? unauthorized)
//    {
//        userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//        if (userId != null)
//        {
//            unauthorized = null;
//            return false;
//        }

//        unauthorized = new UnauthorizedResult();
//        return true;
//    }
//}