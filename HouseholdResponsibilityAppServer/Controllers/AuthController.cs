using HouseholdResponsibilityAppServer.Contracts;
using HouseholdResponsibilityAppServer.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace HouseholdResponsibilityAppServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authenticationService;


        public AuthController(IAuthService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                Debug.WriteLine(request.ToString());
                return BadRequest(ModelState);
            }

            var result = await _authenticationService.RegisterAsync(request.Email, request.Username, request.Password, "User");

            if (!result.Success)
            {
                Debug.WriteLine(request.ToString());
                AddErrors(result);
                return BadRequest(ModelState);
            }
            Debug.WriteLine(request.ToString());
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

            // Token beállítása cookie-ban:
            Response.Cookies.Append("token", result.Token, new CookieOptions
            {
                HttpOnly = true,  //a cookie csak a szerver által elérhető
                Secure = true,      //a cookie csak https esetén kerül elküldésre
                SameSite = SameSiteMode.Lax,  //biztonsági beállítás csak akk küldi el a cookiét ha linkre kattintunk pl.
                Expires = DateTime.UtcNow.AddMinutes(1)
            });

            return Ok(new AuthResponse(result.Email, result.UserName, result.Token));
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("token");
            return Ok(new { message = "Logout successful" });
        }


        //az aktuális user adatait fogja szolgáltatni.
        [Authorize]
        [HttpGet("Me")]
        public IActionResult Me()
        {
            var user = HttpContext.User; //tartalmazza a hitelesített user-hez tartozó claimeket.
            if (user == null)
                return Unauthorized();

            //kinyerjük az adatokat
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




    }
}
