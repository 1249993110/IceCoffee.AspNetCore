using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace IceCoffee.AspNetCore.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="HttpContext"/> that surface commonly needed request metadata
    /// without coupling controllers to low-level ASP.NET Core APIs.
    /// </summary>
    public static class HttpContextExtension
    {
        /// <summary>
        /// Returns the remote IP address of the client for the current request.
        /// When the application runs behind a reverse proxy, ensure the
        /// <c>ForwardedHeaders</c> middleware has already resolved the real client IP
        /// before calling this method.
        /// </summary>
        /// <param name="httpContext">The current <see cref="HttpContext"/>.</param>
        /// <returns>
        /// The client's IP address as a string, or <see langword="null"/> if the connection
        /// does not expose a remote endpoint (e.g. unit-test fakes).
        /// </returns>
        public static string? GetRemoteIpAddress(this HttpContext httpContext)
        {
            return httpContext.Connection.RemoteIpAddress?.ToString();
        }

        /// <summary>
        /// Returns the culture name resolved for the current request by ASP.NET Core's
        /// request localisation middleware. Useful for diagnostic logging or for applying
        /// culture-sensitive business logic that cannot rely on <see cref="System.Threading.Thread.CurrentCulture"/>.
        /// </summary>
        /// <param name="httpContext">The current <see cref="HttpContext"/>.</param>
        /// <returns>
        /// The BCP 47 culture tag (e.g. <c>"en-US"</c>), or <see langword="null"/> if
        /// the request localisation feature is not registered in the pipeline.
        /// </returns>
        public static string? GetCulture(this HttpContext httpContext)
        {
            return httpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.Name;
        }

        /// <summary>
        /// Reconstructs the fully qualified absolute URL of the current request, including
        /// scheme, host, path base, path, and query string. Useful for generating self-referential
        /// links (e.g. pagination, OAuth callbacks) without hard-coding the server's public address.
        /// </summary>
        /// <param name="httpContext">The current <see cref="HttpContext"/>.</param>
        /// <returns>The absolute URL of the current request as a string.</returns>
        public static string GetCurrentUri(this HttpContext httpContext)
        {
            var request = httpContext.Request;
            return request.Scheme + Uri.SchemeDelimiter + request.Host + request.PathBase + request.Path + request.QueryString;
        }
    }
}