using System.ComponentModel.DataAnnotations;
using DotNetFiveApiDemo.Application.User.Identity;

namespace DotNetFiveApiDemo.Application.User.DTOs
{
    public class UserDto
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; init; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; init; }

        [Required(ErrorMessage = "Email confirmation is required.")]
        public bool IsEmailConfirmed { get; init; }

        [Required(ErrorMessage = "Address is required.")]
        public Address Address { get; init; }
    }
}