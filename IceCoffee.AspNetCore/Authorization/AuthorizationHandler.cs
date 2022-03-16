using IceCoffee.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace IceCoffee.AspNetCore.Authorization
{
    /// <summary>
    /// 全局授权处理器
    /// </summary>
    public class AuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly ILogger<AuthorizationHandler> _logger;

        public AuthorizationHandler(ILogger<AuthorizationHandler> logger)
        {
            this._logger = logger;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            try
            {
                var identity = context.User.Identity;
                if (identity == null || identity.IsAuthenticated == false)
                {
                    context.Fail();
                    return Task.CompletedTask;
                }

                if(identity.AuthenticationType == AuthenticationSchemes.ApiKeyAuthenticationSchemeName)
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }

                if (context.Resource is HttpContext httpContext)
                {
                    var routeData = httpContext.GetRouteData();

                    if (routeData.Values.TryGetValue("area", out var area))
                    {
                        string? areas = context.User.FindFirst(Jwt.JwtRegisteredClaimNames.Areas)?.Value;
                        if (areas == null)
                        {
                            context.Fail();
                            return Task.CompletedTask;
                        }

                        bool isInArea = false;
                        int index = 0;
                        foreach (var item in areas.Split(','))
                        {
                            if (item == (area as string))
                            {
                                isInArea = true;
                                break;
                            }

                            ++index;
                        }

                        if (isInArea == false)
                        {
                            context.Fail();
                            return Task.CompletedTask;
                        }

                        if (requirement.RequireHttpMethods)
                        {
                            string? httpMethods = context.User.FindFirst(Jwt.JwtRegisteredClaimNames.HttpMethods)?.Value;
                            if (httpMethods == null)
                            {
                                context.Fail();
                                return Task.CompletedTask;
                            }

                            var httpMethodsArray = httpMethods.Split(';')[index];
                            if (httpMethodsArray == "*")
                            {
                                context.Succeed(requirement);
                                return Task.CompletedTask;
                            }

                            if (httpMethodsArray.Split(',').Contains(httpContext.Request.Method) == false)
                            {
                                context.Fail();
                                return Task.CompletedTask;
                            }
                            else
                            {
                                context.Succeed(requirement);
                                return Task.CompletedTask;
                            }
                        }
                        else
                        {
                            context.Succeed(requirement);
                            return Task.CompletedTask;
                        }
                    }
                    else
                    {
                        context.Succeed(requirement);
                    }
                }
            }
            catch (Exception ex)
            {
                context.Fail();
                _logger.LogWarning(ex, "Error in JwtAuthorizationHandler.HandleRequirementAsync");
            }

            return Task.CompletedTask;
        }
    }
}