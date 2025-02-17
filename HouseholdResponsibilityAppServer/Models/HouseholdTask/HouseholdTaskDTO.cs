namespace HouseholdResponsibilityAppServer.Models.HouseholdTasks
{
    public class HouseholdTaskDTO
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
        public int GroupId { get; set; }
        public bool Priority { get; set; }
    }
}
