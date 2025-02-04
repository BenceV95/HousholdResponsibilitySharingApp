using HouseholdResponsibilityAppServer.Models.Task;
using System.Diagnostics.Eventing.Reader;

namespace HouseholdResponsibilityAppServer.Models.ScheduledTasks
{
    public class ScheduledTask
    {
        public int ScheduledTaskId { get; set; }
        public HouseholdTask HouseholdTask { get; set; }
        public User CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Repeat Repeat { get; set; }
        public DateTime EventDate { get; set; }
        //not sure what these are used for
        public int DayOfWeek { get; set; }
        public int DayOfMonth { get; set; }
        //-----------------------------------------
        public bool AtSpecificTime { get; set; } //if true, we can get the time from the EventDate
        //for sprint 1 it can be only assigned to 1 person
        public User AssignedTo { get; set; }

    }
}
