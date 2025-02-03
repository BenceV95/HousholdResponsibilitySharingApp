namespace HouseholdResponsibilityAppServer.Models.Task
{
    public class Task
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int GroupId { get; set; }
        // if its a bool, change name later?
        public bool Priority { get; set; }

    }
}
