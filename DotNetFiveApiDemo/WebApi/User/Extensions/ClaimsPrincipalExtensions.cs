using System.Security.Claims;

namespace DotNetFiveApiDemo.WebApi.User.Extensions
{
    internal static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}