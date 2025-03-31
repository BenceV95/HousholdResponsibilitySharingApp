using HouseholdResponsibilityAppServer.Models.Households;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HouseholdResponsibilityAppServer.Models.Users
{
    [Index(nameof(Email), IsUnique = true)]
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Household? Household { get; set; }
    }
}
