namespace HouseholdResponsibilityAppServer.Models.Histories
{
    public class CreateHistoryRequest
    {
        public int ScheduledTaskId { get; set; }
        public DateTime CompletedAt { get; set; }
        public string CompletedByUserId { get; set; }
        public bool Outcome { get; set; }

        public int HouseholdId { get; set; }

    }
}
