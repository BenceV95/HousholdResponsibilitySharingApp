using HouseholdResponsibilityAppServer.Models.Households;
using HouseholdResponsibilityAppServer.Models.ScheduledTasks;
using HouseholdResponsibilityAppServer.Models.Users;

namespace HouseholdResponsibilityAppServer.Models.Histories
{
    public class History
    {
        public int HistoryId { get; set; }
        public ScheduledTask ScheduledTask { get; set; }
        public DateTime CompletedAt { get; set; }
        public User CompletedBy { get; set; }
        public bool Outcome {  get; set; } //name is not really declarative (IsCompleted??)
        public Household Household { get; set; }

    }
}
