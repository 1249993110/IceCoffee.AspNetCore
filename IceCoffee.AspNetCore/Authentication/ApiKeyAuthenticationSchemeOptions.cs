using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace IceCoffee.AspNetCore.Authentication
{
    /// <summary>
    /// Api 认证选项
    /// </summary>
    public class ApiKeyAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public string AccessToken { get; set; }
    }
}