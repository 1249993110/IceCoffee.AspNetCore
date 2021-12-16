using IceCoffee.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace IceCoffee.AspNetCore.Extensions
{
    public static class AuthenticationBuilderExtension
    {
        /// <summary>
        /// 添加 ApiKey 认证
        /// </summary>
        /// <param name="authenticationBuilder"></param>
        /// <param name="apiKeyOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddApiKeyAuthentication(this AuthenticationBuilder authenticationBuilder,
            Action<ApiKeyAuthenticationSchemeOptions> apiKeyOptions)
        {
            return authenticationBuilder.AddScheme<ApiKeyAuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
                AuthenticationSchemes.ApiKeyAuthenticationSchemeName, apiKeyOptions);
        }
    }
}
