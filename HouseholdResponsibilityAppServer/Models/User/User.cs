using HouseholdResponsibilityAppServer.Models.Households;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace HouseholdResponsibilityAppServer.Models.Users
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int? HouseholdId { get; set; }
        public Household? Household { get; set; }
    }
}
