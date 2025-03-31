namespace HouseholdResponsibilityAppServer.Models.ScheduledTasks
{
    public class UpdateScheduledTaskRequest
    {
        public Repeat Repeat { get; set; }
        public DateTime EventDate { get; set; }
        public bool AtSpecificTime { get; set; }
        public int AssignedToUserId { get; set; }
    }
}
