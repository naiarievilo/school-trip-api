using System.ComponentModel.DataAnnotations;

namespace DotNetFiveApiDemo.WebApi.User.DTOs
{
    public class SignUpUserRequest
    {
        [EmailAddress(ErrorMessage = "Email provided must be valid.")]
        public string Email { get; init; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; init; }

        [Required(ErrorMessage = "Password confirmation is required.")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; init; }

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; init; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; init; }
    }
}