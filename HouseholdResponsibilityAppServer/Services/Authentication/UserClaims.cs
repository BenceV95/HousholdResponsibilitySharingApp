﻿namespace HouseholdResponsibilityAppServer.Services.Authentication
{
    public class UserClaims
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? HouseholdId { get; set; }
        public List<string> Roles { get; set; }

    }

}
