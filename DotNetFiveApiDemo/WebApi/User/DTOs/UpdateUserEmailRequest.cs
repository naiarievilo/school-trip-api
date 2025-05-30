using System.ComponentModel.DataAnnotations;

namespace DotNetFiveApiDemo.WebApi.User.DTOs
{
    public class UpdateUserEmailRequest
    {
        [EmailAddress(ErrorMessage = "New email provided must be valid.")]
        public string NewEmail { get; init; }
    }
}