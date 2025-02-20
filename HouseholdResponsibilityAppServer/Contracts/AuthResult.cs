namespace HouseholdResponsibilityAppServer.Contracts
{
    public record AuthResult(
     bool Success,
     string Email,
     string UserName,
     string Token,
     string UserId,
     int? HouseholdId)
    {
        //Error code - error message
        public readonly Dictionary<string, string> ErrorMessages = new();
    }
}
