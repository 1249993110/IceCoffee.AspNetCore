using IceCoffee.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace IceCoffee.AspNetCore.Authorization
{
    /// <summary>
    /// 区域授权要求
    /// </summary>
    public class AreaAuthorizationRequirement : AuthorizationHandler<AreaAuthorizationRequirement, HttpContext>, IAuthorizationRequirement
    {
        /// <summary>
        /// Makes a decision if authorization is allowed based on a specific requirement.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The requirement to evaluate.</param>
        /// <param name="httpContext">The optional resource to evaluate the requirement against.</param>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AreaAuthorizationRequirement requirement, HttpContext httpContext)
        {
            var identity = context.User.Identity;
            if (identity == null || identity.IsAuthenticated == false)
            {
                return Task.CompletedTask;
            }

            if (context.User.Identities.Any(i => i.AuthenticationType == AuthenticationSchemes.ApiKeyAuthenticationSchemeName))
            {
                context.Succeed(this);
                return Task.CompletedTask;
            }

            var routeData = httpContext.GetRouteData();

            if (routeData.Values.TryGetValue("area", out var area))
            {
                string? areas = context.User.FindFirst(RegisteredClaimNames.Areas)?.Value;
                if (areas == null)
                {
                    return Task.CompletedTask;
                }
                else if (areas.Split(',').Contains(area))
                {
                    context.Succeed(this);
                }
            }
            else
            {
                context.Succeed(this);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{nameof(AreaAuthorizationRequirement)}: Requires user has been allow access current area.";
        }
    }
}