namespace HouseholdResponsibilityAppServer.Models.Task
{
    public class HouseholdTask
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public User CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Group Group { get; set; }
        // if its a bool, change name later?
        public bool Priority { get; set; }

    }
}
