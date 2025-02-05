namespace HouseholdResponsibilityAppServer.Models
{
    public class Household
    {
        public int HouseholdId { get; set; }
        public string Name { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public User? CreatedByUser { get; set; }

        public List<User> Users { get; set; } = new();
    }
}
