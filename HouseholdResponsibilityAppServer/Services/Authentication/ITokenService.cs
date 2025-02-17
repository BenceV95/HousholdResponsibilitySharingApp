using Microsoft.AspNetCore.Identity;

namespace HouseholdResponsibilityAppServer.Services.Authentication
{
    public interface ITokenService
    {
        string CreateToken(IdentityUser user, string role);
    }
}
