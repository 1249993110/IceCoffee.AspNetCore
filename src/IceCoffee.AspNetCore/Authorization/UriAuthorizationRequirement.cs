using IceCoffee.AspNetCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Security.Claims;

namespace IceCoffee.AspNetCore.Authorization
{
    /// <summary>
    /// Uri授权要求
    /// </summary>
    public class UriAuthorizationRequirement : AuthorizationHandler<UriAuthorizationRequirement, HttpContext>, IAuthorizationRequirement
    {
        /// <summary>
        /// Makes a decision if authorization is allowed based on a specific requirement.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The requirement to evaluate.</param>
        /// <param name="httpContext">The optional resource to evaluate the requirement against.</param>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UriAuthorizationRequirement requirement, HttpContext httpContext)
        {
            if (context.User.Identities.Any(i => i.AuthenticationType == Authentication.AuthenticationSchemes.ApiKeyAuthenticationScheme))
            {
                context.Succeed(this);
                return Task.CompletedTask;
            }

            foreach (var claim in context.User.Claims)
            {
                if (claim.Type.StartsWith(RegisteredClaimNames.PermissionPrefix))
                {
                    string uri = claim.Type.Substring(RegisteredClaimNames.PermissionPrefix.Length);
                    string allowedHttpMethods = claim.Value;

                    if (uri == "/" || (httpContext.Request.PathBase + httpContext.Request.Path).StartsWithSegments(new PathString(uri)))
                    {
                        if (allowedHttpMethods == HttpMethods.Any || allowedHttpMethods.Split(',').Contains(httpContext.Request.Method))
                        {
                            context.Succeed(this);
                            return Task.CompletedTask;
                        }
                    }
                }
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{nameof(UriAuthorizationRequirement)}: Requires user has been allow access current resources.";
        }
    }
}