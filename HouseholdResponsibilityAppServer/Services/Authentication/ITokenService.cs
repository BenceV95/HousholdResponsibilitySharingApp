using HouseholdResponsibilityAppServer.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace HouseholdResponsibilityAppServer.Services.Authentication
{
    public interface ITokenService
    {
        string CreateToken(User user, string role);
    }
}
