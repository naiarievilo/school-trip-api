using System;
using DotNetFiveApiDemo.Application.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNetFiveApiDemo.Application.Auth.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MatchesUserId : Attribute, IAuthorizationFilter
    {
        private readonly string _routeParameterName;

        public MatchesUserId(string routeParameterName = "userId")
        {
            _routeParameterName = routeParameterName;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Check if the user is authenticated
            if (!context.HttpContext.User.Identity!.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Get the user ID from the JWT token
            var userId = context.HttpContext.User.GetUserId();
            if (userId == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            // Try to get the user ID from the route data
            if (!context.RouteData.Values.TryGetValue(_routeParameterName, out var routeUserIdObj))
            {
                // Parameter wasn't found in the route, so we can't validate ownership
                context.Result = new BadRequestResult();
                return;
            }

            var routeUserId = routeUserIdObj!.ToString();

            // Validate that the user ID in the route matches the user ID in the token
            if (!string.Equals(userId, routeUserId, StringComparison.OrdinalIgnoreCase))
                context.Result = new ForbidResult();
        }
    }
}