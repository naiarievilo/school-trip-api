using System.ComponentModel.DataAnnotations;

namespace DotNetFiveApiDemo.WebApi.User.DTOs
{
    public class UnverifiedUserRequest
    {
        [EmailAddress(ErrorMessage = "Email provided must be valid.")]
        public string Email { get; init; }
    }
}