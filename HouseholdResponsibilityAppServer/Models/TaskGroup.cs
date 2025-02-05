using System.ComponentModel.DataAnnotations;

namespace HouseholdResponsibilityAppServer.Models
{
    public class TaskGroup
    {
        [Key]
        public int GroupId { get; set; }
        public string Name { get; set; }

    }
}
