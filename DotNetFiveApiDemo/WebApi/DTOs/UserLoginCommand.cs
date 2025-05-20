using System.ComponentModel.DataAnnotations;
using DotNetFiveApiDemo.Application.Common.Validation;

namespace DotNetFiveApiDemo.WebApi.DTOs
{
    public class UserLoginCommand
    {
        [AtLeastOneOf("Email", "UserName")] public string Email { get; init; }

        [AtLeastOneOf("Email", "UserName")] public string UserName { get; init; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; init; }
    }
}