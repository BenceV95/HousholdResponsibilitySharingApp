using HouseholdResponsibilityAppServer.Contracts;

namespace HouseholdResponsibilityAppServer.Services.Authentication
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegistrationRequest request, string role);
        Task<AuthResult> LoginAsync(string email, string password);
        public UserClaims GetClaimsFromHttpContext(HttpContext context);
    }
}
