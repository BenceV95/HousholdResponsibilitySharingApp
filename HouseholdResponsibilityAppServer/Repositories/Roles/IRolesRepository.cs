using Microsoft.AspNetCore.Identity;

namespace HouseholdResponsibilityAppServer.Repositories.Roles
{
    public interface IRoleRepository
    {
        Task<IEnumerable<IdentityRole>> GetAllRolesAsync();
        Task<IdentityRole> GetRoleByUserIdAsync(string userId);
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<IdentityResult> DeleteRoleAsync(string roleId);
    }
}
