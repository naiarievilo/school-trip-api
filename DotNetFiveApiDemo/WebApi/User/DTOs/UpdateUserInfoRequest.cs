using System.ComponentModel.DataAnnotations;
using DotNetFiveApiDemo.Infrastructure.Data.Configuration;

namespace DotNetFiveApiDemo.WebApi.User.DTOs
{
    public class UpdateUserInfoRequest
    {
        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(AppUserConfiguration.FirstNameMaxLength, ErrorMessage = "First name cannot exceed 64 characters.")]
        public string FirstName { get; init; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(AppUserConfiguration.LastNameMaxLength, ErrorMessage = "Last name cannot exceed 64 characters.")]
        public string LastName { get; init; }

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(AppUserConfiguration.StreetMaxLength, ErrorMessage = "Street cannot exceed 256 characters.")]
        public string Street { get; init; }

        [Required(ErrorMessage = "Number is required.")]
        [MaxLength(AppUserConfiguration.NumberMaxLength, ErrorMessage = "Number cannot exceed 32 characters.")]
        public string Number { get; init; }

        [Required(ErrorMessage = "City is required.")]
        [MaxLength(AppUserConfiguration.CityMaxLength, ErrorMessage = "City cannot exceed 64 characters.")]
        public string City { get; init; }

        [Required(ErrorMessage = "State is required.")]
        [MaxLength(AppUserConfiguration.StateMaxLength, ErrorMessage = "State cannot be longer than 64 characters.")]
        public string State { get; init; }

        [Required(ErrorMessage = "Country is required.")]
        [MaxLength(AppUserConfiguration.CountryMaxLength,
            ErrorMessage = "Country cannot be longer than 64 characters.")]
        public string Country { get; init; }

        [Required(ErrorMessage = "Postal code is required.")]
        [MaxLength(AppUserConfiguration.PostalCodeMaxLength,
            ErrorMessage = "Postal code cannot be longer than 32 characters.")]
        public string PostalCode { get; init; }
    }
}