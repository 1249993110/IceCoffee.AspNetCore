﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace IceCoffee.AspNetCore.Authentication
{
    /// <summary>
    /// ApiKey 认证处理器
    /// </summary>
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationSchemeOptions>
    {
        private static readonly AuthenticationTicket _authTicket;

        public const string HttpRequestHeaderName = "access-token";

        static ApiKeyAuthenticationHandler()
        {
            // 声明
            var claims = Array.Empty<Claim>();

            // 身份证
            var claimsIdentity = new ClaimsIdentity(claims, AuthenticationSchemes.ApiKeyAuthenticationScheme);

            // 表示一个人, 把身份证给这个人
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // 将凭证与人关联
            _authTicket = new AuthenticationTicket(claimsPrincipal, AuthenticationSchemes.ApiKeyAuthenticationScheme);
        }

#if NET8_0_OR_GREATER
        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }
#else
        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }
#endif

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Context.GetEndpoint()?.Metadata.GetMetadata<IAllowAnonymous>() != null)
            {
                // 返回 Fail 会写一条 Info 日志, 参考asp框架默认实现, 其在没有 cookie 时也返回 NoResult
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            if (string.IsNullOrEmpty(Options.AccessToken))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
                // return Task.FromResult(AuthenticateResult.Fail("The configured access token is null or empty"));
            }

            if (Request.Headers.TryGetValue(HttpRequestHeaderName, out var value) && value == Options.AccessToken)
            {
                return Task.FromResult(AuthenticateResult.Success(_authTicket));
            }

            if (Request.Query[HttpRequestHeaderName] == Options.AccessToken)
            {
                return Task.FromResult(AuthenticateResult.Success(_authTicket));
            }

            return Task.FromResult(AuthenticateResult.NoResult());
        }
    }
}