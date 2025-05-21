using System.ComponentModel.DataAnnotations;

namespace DotNetFiveApiDemo.WebApi.DTOs
{
    public class UserUpdateCommand
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; init; }

        [EmailAddress(ErrorMessage = "Must be a valid email address.")]
        public string Email { get; init; }

        public string CurrentPassword { get; init; }

        [DataType(DataType.Password)] public string NewPassword { get; init; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "New passwords do not match.")]
        public string ConfirmNewPassword { get; init; }

        [MaxLength(256, ErrorMessage = "Street cannot exceed 256 characters.")]
        public string Street { get; init; }

        public string Number { get; init; }

        [MaxLength(256, ErrorMessage = "City cannot exceed 256 characters.")]
        public string City { get; init; }

        [MaxLength(256, ErrorMessage = "State cannot be longer than 256 characters.")]
        public string State { get; init; }

        [MaxLength(256, ErrorMessage = "Country cannot be longer than 256 characters.")]
        public string Country { get; init; }

        [MaxLength(256, ErrorMessage = "Zip Code cannot be longer than 256 characters.")]
        public string ZipCode { get; init; }
    }
}