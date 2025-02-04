using HouseholdResponsibilityAppServer.Models.ScheduledTasks;

namespace HouseholdResponsibilityAppServer.Models.Histories
{
    public class History
    {
        public int HistoryId { get; set; }
        public ScheduledTask ScheduledTask { get; set; }
        public DateTime CompletedAt { get; set; }
        public User CompletedBy { get; set; }
        public bool Action {  get; set; } //name is not really declarative (IsCompleted??)

    }
}
