using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SchoolTripApi.WebApi.Account.Extensions;

namespace SchoolTripApi.WebApi.Account.Filters;

[AttributeUsage(AttributeTargets.Method)]
internal class MatchesAuthenticatedAccountId(string routeParameterName = "accountId") : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.User.Identity!.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userId = context.HttpContext.User.GetUserId();
        if (userId is null)
        {
            context.Result = new ForbidResult();
            return;
        }

        if (!context.RouteData.Values.TryGetValue(routeParameterName, out var routeUserIdObj))
            throw new ApplicationException(
                $"{context.RouteData}: Account's ID router parameter must be the equal to '_routeParameterName' of 'MatchesAuthenticatedUserId' attribute.");

        var routeUserId = routeUserIdObj!.ToString();
        if (!string.Equals(userId, routeUserId, StringComparison.OrdinalIgnoreCase))
            context.Result = new ForbidResult();
    }
}