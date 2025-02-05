using HouseholdResponsibilityAppServer.Models.Households;
using HouseholdResponsibilityAppServer.Models.ScheduledTasks;
using HouseholdResponsibilityAppServer.Models.Users;

namespace HouseholdResponsibilityAppServer.Models.Histories
{
    public class History
    {
        public int HistoryId { get; set; }
        public int ScheduledTaskId { get; set; }
        public ScheduledTask ScheduledTask { get; set; }
        public DateTime CompletedAt { get; set; }
        public int CompletedById { get; set; }
        public User CompletedBy { get; set; }
        public bool Outcome {  get; set; }
        public int HouseholdId { get; set; }
        public Household Household { get; set; }




    }
}
