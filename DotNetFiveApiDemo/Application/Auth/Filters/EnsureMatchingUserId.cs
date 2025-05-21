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
            if (!context.HttpContext.User.Identity!.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userId = context.HttpContext.User.GetUserId();
            if (userId == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            if (!context.RouteData.Values.TryGetValue(_routeParameterName, out var routeUserIdObj))
            {
                context.Result = new BadRequestResult();
                return;
            }

            var routeUserId = routeUserIdObj!.ToString();
            if (!string.Equals(userId, routeUserId, StringComparison.OrdinalIgnoreCase))
                context.Result = new ForbidResult();
        }
    }
}