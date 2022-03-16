using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;

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
    }
}
