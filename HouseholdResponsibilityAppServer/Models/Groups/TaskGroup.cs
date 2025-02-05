using HouseholdResponsibilityAppServer.Models.Households;
using System.ComponentModel.DataAnnotations;

namespace HouseholdResponsibilityAppServer.Models.Groups
{
    public class TaskGroup
    {
        [Key]
        public int GroupId { get; set; }
        public string Name { get; set; }
        public Household Household { get; set; }

        public int HouseholdId { get; set; }

        public static IEnumerable<TaskGroup> CreateDefaultGroups()
        {
            return new List<TaskGroup>
            {
                new TaskGroup(){GroupId = 0, Name = "🌳"},
                new TaskGroup(){GroupId = 1, Name = "🍽"},
                new TaskGroup(){GroupId = 2, Name = "👕"},
                new TaskGroup(){GroupId = 3, Name = "🚽"},
                new TaskGroup(){GroupId = 4, Name = "🛏"},
                new TaskGroup(){GroupId = 5, Name = "🧺"},
                new TaskGroup(){GroupId = 6, Name = "🛒"},
                new TaskGroup(){GroupId = 7, Name = "🛁"},
                new TaskGroup(){GroupId = 8, Name = "🐶"},
                new TaskGroup(){GroupId = 9, Name = "🚘"},
            };


        }
    }
    
}
