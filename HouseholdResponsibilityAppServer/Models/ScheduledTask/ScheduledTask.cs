
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
        //note: this seems overkill, we should use an enum
        public bool Daily { get; set; }
        public bool Weekly { get; set; }
        public bool Monthly { get; set; }
        public bool NoRepeat { get; set; }
        //-------------------------------
        public DateTime EventDate { get; set; }
        public int DayOfWeek { get; set; }
        public int DayOfMonth { get; set; }
        //maybe we could get this date from the event date, since that is DateTime...
        public DateTime SpecificTime { get; set; }
        //for sprint 1 it can be only assigned to 1 person
        public User AssignedTo { get; set; }

    }
}
