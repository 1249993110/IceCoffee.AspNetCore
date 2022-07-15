using IceCoffee.AspNetCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace IceCoffee.AspNetCore.Extensions
{
    public static class HttpContextExtension
    {
        public static string GetRequestId(this HttpContext httpContext)
        {
            return Activity.Current?.Id ?? httpContext.TraceIdentifier;
        }

        public static string? GetRemoteIpAddress(this HttpContext httpContext)
        {
            return httpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
        }

        public static string? GetCulture(this HttpContext httpContext)
        {
            return httpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.Name;
        }
    }
}