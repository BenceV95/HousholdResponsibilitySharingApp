﻿namespace HouseholdResponsibilityAppServer.Models.HouseholdTasks
{
    public class CreateHouseholdTaskRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int GroupId { get; set; }
        public bool Priority { get; set; }
    }
}
