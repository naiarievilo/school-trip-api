using System.ComponentModel.DataAnnotations;

namespace DotNetFiveApiDemo.WebApi.User.DTOs
{
    public class ResetPasswordRequest : UnverifiedUserRequest
    {
        [Required(ErrorMessage = "Reset code is required.")]
        public string ResetCode { get; init; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; init; }

        [Required(ErrorMessage = "New password confirmation is required.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmNewPassword { get; init; }
    }
}