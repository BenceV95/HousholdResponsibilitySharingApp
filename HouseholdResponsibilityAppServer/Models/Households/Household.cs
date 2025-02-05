using HouseholdResponsibilityAppServer.Models.Groups;
using HouseholdResponsibilityAppServer.Models.Histories;
using HouseholdResponsibilityAppServer.Models.Task;
using HouseholdResponsibilityAppServer.Models.Users;

namespace HouseholdResponsibilityAppServer.Models.Households
{
    public class Household
    {
        public int HouseholdId { get; set; }
        public string Name { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public User? CreatedByUser { get; set; }

        public List<User> Users { get; set; } = new();

        public List<TaskGroup> Groups { get; set; } = new();

        public List<HouseholdTask> HouseholdTasks { get; set; } = new();

        public List<History> Histories { get; set; } = new();




    }
}
