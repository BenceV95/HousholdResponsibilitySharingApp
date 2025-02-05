using HouseholdResponsibilityAppServer.Models.Groups;
using HouseholdResponsibilityAppServer.Models.Households;
using HouseholdResponsibilityAppServer.Models.Users;

namespace HouseholdResponsibilityAppServer.Models.Task
{
    public class HouseholdTask
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public User CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public TaskGroup Group { get; set; }
        public bool Priority { get; set; }
        public Household Household { get; set; }


    }
}
