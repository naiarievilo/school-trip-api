using System.ComponentModel.DataAnnotations;

namespace DotNetFiveApiDemo.WebApi.DTOs
{
    public class UserCreationCommand
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; init; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Must be a valid email address")]
        public string Email { get; init; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; init; }

        [Required(ErrorMessage = "Password confirmation is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; init; }

        [Required(ErrorMessage = "Street is required")]
        [MaxLength(256, ErrorMessage = "Street cannot exceed 256 characters")]
        public string Street { get; init; }

        [Required(ErrorMessage = "Number is required")]
        public string Number { get; init; }

        [Required(ErrorMessage = "City is required")]
        [MaxLength(256, ErrorMessage = "City cannot exceed 256 characters")]
        public string City { get; init; }

        [Required(ErrorMessage = "State is required")]
        [MaxLength(256, ErrorMessage = "State cannot be longer than 256 characters")]
        public string State { get; init; }

        [Required(ErrorMessage = "Country is required")]
        [MaxLength(256, ErrorMessage = "Country cannot be longer than 256 characters")]
        public string Country { get; init; }

        [Required(ErrorMessage = "Zip code is required")]
        [MaxLength(256, ErrorMessage = "Zip Code cannot be longer than 256 characters")]
        public string ZipCode { get; init; }
    }
}