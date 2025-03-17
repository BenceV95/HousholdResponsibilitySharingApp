using HouseholdResponsibilityAppServer.Context;
using HouseholdResponsibilityAppServer.Contracts;
using HouseholdResponsibilityAppServer.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HouseholdResponsibilityAppServer.Services.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly HouseholdResponsibilityAppContext _context;


        public AuthService(UserManager<User> userManager, ITokenService tokenService,
            RoleManager<IdentityRole> roleManager, HouseholdResponsibilityAppContext context)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<AuthResult> RegisterAsync(RegistrationRequest request, string role)
        {
            var user = new User
            {
                UserName = request.Username,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return FailedRegistration(result, request.Email, request.Username);
            }

            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                return new AuthResult(false, request.Email, request.Username, "", null)
                {
                    ErrorMessages = { { "RoleError", "The specified role does not exist." } }
                };
            }

            await _userManager.AddToRoleAsync(user, role);
            return new AuthResult(true, request.Email, request.Username, "", null);
        }

        private static AuthResult FailedRegistration(IdentityResult result, string email, string username)
        {
            var authResult = new AuthResult(false, email, username, "", null);

            foreach (var error in result.Errors)
            {
                authResult.ErrorMessages.Add(error.Code, error.Description);
            }

            return authResult;
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            var managedUser = await _userManager.FindByEmailAsync(email);
            if (managedUser == null)
            {
                return InvalidEmail(email);
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, password);
            if (!isPasswordValid)
            {
                return InvalidPassword(email, managedUser.UserName);
            }

            // this is for returning the household id
            _context.Entry(managedUser).Reference(u => u.Household).Load();

            var accessToken = await _tokenService.CreateToken(managedUser);

            return new AuthResult(true, managedUser.Email, managedUser.UserName, accessToken, managedUser.Household?.HouseholdId);
        }


        private static AuthResult InvalidEmail(string email)
        {
            var result = new AuthResult(false, email, "", "", null);
            result.ErrorMessages.Add("Bad credentials", "Invalid email");
            return result;
        }

        private static AuthResult InvalidPassword(string email, string userName)
        {
            var result = new AuthResult(false, email, userName, "", null);
            result.ErrorMessages.Add("Bad credentials", "Invalid password");
            return result;
        }

        public UserClaims GetClaimsFromHttpContext(HttpContext context)
        {

           string userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = context.User;

            var householdId = user.FindFirst("householdId")?.Value;

            return new UserClaims
            {
                UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                UserName = user.FindFirst(ClaimTypes.Name)?.Value,
                Email = user.FindFirst(ClaimTypes.Email)?.Value,
                HouseholdId = string.IsNullOrEmpty(householdId) ? null : householdId, // Null if empty
                Roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList()
            };
        }

    }
}
