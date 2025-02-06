namespace HouseholdResponsibilityAppServer.Models.Invitations
{
    public class InviteUserDto
    {
        public string Email { get; set; }
        public string InvitedByUsername { get; set; }
        public int HouseholdId { get; set; }
    }
}
