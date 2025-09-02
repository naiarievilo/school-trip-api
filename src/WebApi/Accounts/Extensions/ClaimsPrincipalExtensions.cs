using System.Security.Claims;

namespace SchoolTripApi.WebApi.Accounts.Extensions;

internal static class ClaimsPrincipalExtensions
{
    public static string? GetAccountId(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}