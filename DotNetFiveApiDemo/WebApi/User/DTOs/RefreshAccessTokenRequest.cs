using System.ComponentModel.DataAnnotations;

namespace DotNetFiveApiDemo.WebApi.User.DTOs
{
    public class RefreshAccessTokenRequest
    {
        [Required(ErrorMessage = "Refresh token is required.")]
        public string RefreshToken { get; set; }
    }
}