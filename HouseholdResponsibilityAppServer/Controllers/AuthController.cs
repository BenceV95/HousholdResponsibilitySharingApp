﻿using HouseholdResponsibilityAppServer.Contracts;
using HouseholdResponsibilityAppServer.Repositories.UserRepo;
using HouseholdResponsibilityAppServer.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        const int _expirationTimeInMinutes = 60;


        public AuthController(
            IAuthService authenticationService,
            IUserRepository userRepository,
            ITokenService tokenService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
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

            //when the user logs in, add cookies, and to the cookie add the token

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


        // call this endpoint from the frontend after the token needs to be updated
        [Authorize]
        [HttpGet("update-token")]
        public async Task<IActionResult> RefreshToken()
        {

            UserClaims userClaims = _authenticationService.GetClaimsFromHttpContext(HttpContext);

            // Fetch updated user details (now including HouseholdId)
            var user = await _userRepository.GetUserByIdAsync(userClaims.UserId);



            Response.Cookies.Delete("token"); // Ensure old token is removed before adding the new one



            // Generate new token with updated HouseholdId
            var token = await _tokenService.CreateToken(user);


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