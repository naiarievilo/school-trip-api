using System.ComponentModel.DataAnnotations;

namespace DotNetFiveApiDemo.WebApi.User.DTOs
{
    public class SignInUserRequest
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email provided must be valid.")]
        public string Email { get; init; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; init; }
    }
}