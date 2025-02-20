using HouseholdResponsibilityAppServer.Context;
using HouseholdResponsibilityAppServer.Contracts;
using HouseholdResponsibilityAppServer.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        public async Task<AuthResult> RegisterAsync(string email, string username, string password, string role)
        {
            var user = new User { UserName = username, Email = email }; //ez csak létrehozza de nincs mentve
            var result = await _userManager.CreateAsync(user, password); //ez menti el + hasheli a passwordot is.

            if (!result.Succeeded)
            {
                return FailedRegistration(result, email, username);
            }

            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                return new AuthResult(false, email, username, "" , "", null)
                {
                    ErrorMessages = { { "RoleError", "The specified role does not exist." } }
                };
            }

            await _userManager.AddToRoleAsync(user, role);
            return new AuthResult(true, email, username, "", "", null);
        }

        private static AuthResult FailedRegistration(IdentityResult result, string email, string username)
        {
            var authResult = new AuthResult(false, email, username, "", "", null);

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


            _context.Entry(managedUser).Reference(u => u.Household).Load();

            var roles = await _userManager.GetRolesAsync(managedUser);
            var accessToken = _tokenService.CreateToken(managedUser, roles.FirstOrDefault());

            return new AuthResult(true, managedUser.Email, managedUser.UserName, accessToken, managedUser.Id, managedUser.Household?.HouseholdId);
        }


        private static AuthResult InvalidEmail(string email)
        {
            var result = new AuthResult(false, email, "", "", "", null);
            result.ErrorMessages.Add("Bad credentials", "Invalid email");
            return result;
        }

        private static AuthResult InvalidPassword(string email, string userName)
        {
            var result = new AuthResult(false, email, userName, "", "", null);
            result.ErrorMessages.Add("Bad credentials", "Invalid password");
            return result;
        }
    }
}
