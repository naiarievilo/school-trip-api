using System;
using DotNetFiveApiDemo.WebApi.User.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNetFiveApiDemo.WebApi.User.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class MatchesAuthenticatedUserId : Attribute, IAuthorizationFilter
    {
        private readonly string _routeParameterName;

        public MatchesAuthenticatedUserId(string routeParameterName = "userId")
        {
            _routeParameterName = routeParameterName;
        }

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

            if (!context.RouteData.Values.TryGetValue(_routeParameterName, out var routeUserIdObj))
                throw new ApplicationException(
                    $"{context.RouteData}: User ID router parameter must be the equal to '_routeParameterName' of 'MatchesAuthenticatedUserId' attribute.");

            var routeUserId = routeUserIdObj!.ToString();
            if (!string.Equals(userId, routeUserId, StringComparison.OrdinalIgnoreCase))
                context.Result = new ForbidResult();
        }
    }
}