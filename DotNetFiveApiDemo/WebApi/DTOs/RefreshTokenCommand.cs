using System.ComponentModel.DataAnnotations;

namespace DotNetFiveApiDemo.WebApi.DTOs
{
    public class RefreshTokenCommand
    {
        [Required(ErrorMessage = "Access token is required")]
        public string AccessToken { get; set; }
        [Required(ErrorMessage = "Refresh token is required")]
        public string RefreshToken { get; set; }
    }
}