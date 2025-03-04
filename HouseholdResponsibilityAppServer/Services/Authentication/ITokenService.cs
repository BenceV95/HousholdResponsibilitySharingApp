using HouseholdResponsibilityAppServer.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace HouseholdResponsibilityAppServer.Services.Authentication
{
    public interface ITokenService
    {
        public Task<string> CreateToken(User user);
    }
}
