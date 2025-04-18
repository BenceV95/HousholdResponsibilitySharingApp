﻿namespace HouseholdResponsibilityAppServer.Models.ScheduledTasks
{
    public class ScheduledTaskDTO
    {
        public int ScheduledTaskId { get; set; }
        public int HouseholdTaskId { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Repeat Repeat { get; set; }
        public DateTime EventDate { get; set; }
        public bool AtSpecificTime { get; set; }
        public string AssignedToUserId { get; set; }
    }
}
