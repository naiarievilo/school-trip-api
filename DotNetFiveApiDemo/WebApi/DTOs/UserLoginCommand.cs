using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DotNetFiveApiDemo.Application.Common.Validation;

namespace DotNetFiveApiDemo.WebApi.DTOs
{
    public class UserLoginCommand
    {
        [AllowNull]
        [AtLeastOneOf("Email", "UserName")]
        public string Email { get; init; }

        [AllowNull]
        [AtLeastOneOf("Email", "UserName")]
        public string UserName { get; init; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; init; }
    }
}