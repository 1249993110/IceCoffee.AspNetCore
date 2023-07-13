using IceCoffee.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;

namespace IceCoffee.AspNetCore.Extensions
{
    public static class AuthenticationBuilderExtension
    {
        /// <summary>
        /// 添加 ApiKey 认证
        /// </summary>
        /// <param name="authenticationBuilder"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddApiKeyAuthentication(this AuthenticationBuilder authenticationBuilder,
            Action<ApiKeyAuthenticationSchemeOptions> configureOptions)
        {
            return authenticationBuilder.AddScheme<ApiKeyAuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
                AuthenticationSchemes.ApiKeyAuthenticationSchemeName, configureOptions);
        }
    }
}