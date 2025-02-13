﻿using HouseholdResponsibilityAppServer.Models.Task;

namespace HouseholdResponsibilityAppServer.Models.ScheduledTasks
{
    public class CreateScheduledTaskRequest
    {
        public int HouseholdTaskId { get; set; }
        public int CreatedByUserId { get; set; } 
        public Repeat Repeat { get; set; }
        public DateTime EventDate { get; set; }
        public bool AtSpecificTime { get; set; }
        public int AssignedToUserId { get; set; }
    }
}
