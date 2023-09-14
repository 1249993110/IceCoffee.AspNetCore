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
                AuthenticationSchemes.ApiKeyAuthenticationScheme, configureOptions);
        }

        /// <summary>
        /// 添加 Basic 认证
        /// </summary>
        /// <param name="authenticationBuilder"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddBasicAuthentication(this AuthenticationBuilder authenticationBuilder,
            Action<BasicAuthenticationSchemeOptions> configureOptions)
        {
            return authenticationBuilder.AddScheme<BasicAuthenticationSchemeOptions, BasicAuthenticationHandler>(
                AuthenticationSchemes.BasicAuthenticationScheme, configureOptions);
        }
    }
}