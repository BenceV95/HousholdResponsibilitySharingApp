﻿namespace HouseholdResponsibilityAppServer.Contracts
{
    public record AuthResponse(string Email, string UserName, string Token, string UserId, int? HouseholdId);
}
