using System.ComponentModel.DataAnnotations;

namespace HouseholdResponsibilityAppServer.Contracts
{
    public record RegistrationRequest(
    [Required] string Email,
    [Required] string Username,
    [Required] string Password);
}
