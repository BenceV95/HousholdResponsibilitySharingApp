﻿namespace HouseholdResponsibilityAppServer.Contracts
{
    public record AuthResponse(string Email, string UserName, int? HouseholdId);
}
