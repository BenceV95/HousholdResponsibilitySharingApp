using HouseholdResponsibilityAppServer.Models.ScheduledTasks;

namespace HouseholdResponsibilityAppServer.Models.Histories
{
    public class CreateHistoryRequest
    {
        public int ScheduledTaskId { get; set; }
        public DateTime CompletedAt { get; set; }
        public int CompletedByUserId { get; set; }
        public bool Action { get; set; }

    }
}
