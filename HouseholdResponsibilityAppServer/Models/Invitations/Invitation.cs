namespace HouseholdResponsibilityAppServer.Models.Invitations
{
    public class Invitation
    {
        public int Id { get; set; }
        public int HouseholdId { get; set; }
        public string Email { get; set; }
        public string InvitedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsAccepted { get; set; } = false;
    }

}
