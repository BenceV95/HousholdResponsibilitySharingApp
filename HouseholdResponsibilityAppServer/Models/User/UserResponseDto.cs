namespace HouseholdResponsibilityAppServer.Models.Users
{
    public class UserResponseDto
    {
        public string UserResponseDtoId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? HouseholdId { get; set; }
    }
}
