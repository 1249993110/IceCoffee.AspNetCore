using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace IceCoffee.AspNetCore.Extensions
{
    public static class HttpContextExtension
    {
        public static string? GetRemoteIpAddress(this HttpContext httpContext)
        {
            return httpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
        }

        public static string? GetCulture(this HttpContext httpContext)
        {
            return httpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.Name;
        }

        /// <summary>
        /// Gets the absolute current url.
        /// </summary>
        public static string GetCurrentUri(this HttpContext httpContext)
        {
            var request = httpContext.Request;
            return request.Scheme + Uri.SchemeDelimiter + request.Host + request.PathBase + request.Path + request.QueryString;
        }
    }
}