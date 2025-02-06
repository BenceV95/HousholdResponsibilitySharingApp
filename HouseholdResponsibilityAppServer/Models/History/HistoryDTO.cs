using HouseholdResponsibilityAppServer.Models.ScheduledTasks;

namespace HouseholdResponsibilityAppServer.Models.Histories
{
    public class HistoryDTO
    {
        public int HistoryId { get; set; }
        public int ScheduledTaskId { get; set; }
        public DateTime CompletedAt { get; set; }
        public int CompletedByUserId { get; set; }
        public bool Outcome { get; set; }

        public int HouseholdId { get; set; }

    }
}
