namespace HouseholdResponsibilityAppServer.DTOs
{
    public class UserResponseDto
    {
        public int UserResponseDtoId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? HouseholdId { get; set; }
    }
}
