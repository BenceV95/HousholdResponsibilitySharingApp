using HouseholdResponsibilityAppServer.Models.Users;

namespace HouseholdResponsibilityAppServer.Services.Authentication
{
    public interface ITokenService
    {
        public Task<string> CreateToken(User user);
    }
}
