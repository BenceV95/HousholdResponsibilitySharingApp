﻿using HouseholdResponsibilityAppServer.Models.Task;

namespace HouseholdResponsibilityAppServer.Models.ScheduledTasks
{
    public class ScheduledTaskDTO
    {
        public int ScheduledTaskId { get; set; }
        public int HouseholdTaskId { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Repeat Repeat { get; set; }
        public DateTime EventDate { get; set; }
        public int DayOfWeek { get; set; }
        public int DayOfMonth { get; set; }
        public bool AtSpecificTime { get; set; }
        public int AssignedToUserId { get; set; }
    }
}
