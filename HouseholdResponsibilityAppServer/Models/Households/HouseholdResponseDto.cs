namespace HouseholdResponsibilityAppServer.Models.Households
{
    public class HouseholdResponseDto
    {
        public int HouseholdResponseDtoId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByUsername { get; set; } // User ID helyett név
    }

}
