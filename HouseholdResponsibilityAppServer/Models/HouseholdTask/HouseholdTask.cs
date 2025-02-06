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

        public int CreatedById { get; set; } // Foreign key for User
        public User CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public int GroupId { get; set; } // Foreign key for TaskGroup
        public TaskGroup Group { get; set; }

        public bool Priority { get; set; }

        public int HouseholdId { get; set; } // Foreign key for Household
        public Household Household { get; set; }



    }
}
