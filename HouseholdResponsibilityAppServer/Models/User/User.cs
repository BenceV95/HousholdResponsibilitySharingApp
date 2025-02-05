using HouseholdResponsibilityAppServer.Models.Households;
namespace HouseholdResponsibilityAppServer.Models.Users
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Household? Household { get; set; }
    }
}
