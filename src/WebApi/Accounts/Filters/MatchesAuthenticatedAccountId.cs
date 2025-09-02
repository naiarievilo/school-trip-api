using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SchoolTripApi.WebApi.Accounts.Extensions;

namespace SchoolTripApi.WebApi.Accounts.Filters;

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

        var accountId = context.HttpContext.User.GetAccountId();
        if (accountId is null)
        {
            context.Result = new ForbidResult();
            return;
        }

        if (!context.RouteData.Values.TryGetValue(routeParameterName, out var accountIdObjectFromRoute))
            throw new ApplicationException(
                $"{context.RouteData}: Account's ID router parameter must be the equal to '_routeParameterName' of 'MatchesAuthenticatedUserId' attribute.");

        var accountIdFromRoute = accountIdObjectFromRoute!.ToString();
        if (!string.Equals(accountId, accountIdFromRoute))
            context.Result = new ForbidResult();
    }
}