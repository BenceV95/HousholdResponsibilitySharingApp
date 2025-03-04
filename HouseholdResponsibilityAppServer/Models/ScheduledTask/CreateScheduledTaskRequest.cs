using HouseholdResponsibilityAppServer.Models.Task;

namespace HouseholdResponsibilityAppServer.Models.ScheduledTasks
{
    public class CreateScheduledTaskRequest
    {
        public int HouseholdTaskId { get; set; }
        public string CreatedByUserId { get; set; } 
        public Repeat Repeat { get; set; }
        public DateTime EventDate { get; set; }
        public bool AtSpecificTime { get; set; }
        public string AssignedToUserId { get; set; }
    }
}
