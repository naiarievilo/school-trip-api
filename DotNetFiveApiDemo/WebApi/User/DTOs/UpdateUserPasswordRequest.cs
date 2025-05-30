using System.ComponentModel.DataAnnotations;

namespace DotNetFiveApiDemo.WebApi.User.DTOs
{
    public class UpdateUserPasswordRequest
    {
        [Required(ErrorMessage = "Current password is required.")]
        public string CurrentPassword { get; init; }

        [Required(ErrorMessage = "New password is required.")]
        public string NewPassword { get; init; }

        [Required(ErrorMessage = "New password confirmation is required.")]
        [Compare(nameof(NewPassword), ErrorMessage = "New passwords do not match.")]
        public string ConfirmNewPassword { get; init; }
    }
}