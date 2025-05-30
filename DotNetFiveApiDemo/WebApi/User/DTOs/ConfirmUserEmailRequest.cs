using System.ComponentModel.DataAnnotations;

namespace DotNetFiveApiDemo.WebApi.User.DTOs
{
    public class ConfirmUserEmailRequest
    {
        [EmailAddress(ErrorMessage = "Email provided must be valid.")]
        public string Email { get; init; }

        [Required(ErrorMessage = "Email confirmation token is required.")]
        public string EmailConfirmationToken { get; init; }
    }
}