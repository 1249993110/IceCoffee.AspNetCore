using IceCoffee.AspNetCore.Authentication;
using IceCoffee.AspNetCore.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace IceCoffee.AspNetCore.Extensions
{
    /// <summary>
    /// Authentication Builder Extension
    /// </summary>
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

        /// <summary>
        /// 添加 Jwt 认证策略到 IServiceCollection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="jwtOptionsSection"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddJwtAuthentication(this IServiceCollection services, IConfigurationSection jwtOptionsSection)
        {
            services.AddOptions<JwtOptions>()
                .Bind(jwtOptionsSection)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            var jwtOptions = jwtOptionsSection.Get<JwtOptions>() ?? throw new Exception("The JWT options not be null."); ;
            var key = Encoding.UTF8.GetBytes(jwtOptions.SecretKey);

            var tokenValidationParams = new TokenValidationParameters()
            {
                // 是否校验安全令牌
                ValidateIssuerSigningKey = true,
                // 是否校验过期时间
                ValidateLifetime = true,
                // 是否校验颁发者
                ValidateIssuer = false,
                // 是否校验被颁发者
                ValidateAudience = false,
                // 安全令牌
                IssuerSigningKey = new SymmetricSecurityKey(key),
                // 缓冲过期时间偏移量
                ClockSkew = TimeSpan.FromSeconds(jwtOptions.ClockSkew),
                // 指示令牌是否必须具有到期值
                RequireExpirationTime = true,

                AuthenticationType = JwtBearerDefaults.AuthenticationScheme,

                NameClaimType = RegisteredClaimNames.UserName,

                RoleClaimType = RegisteredClaimNames.RoleNames
            };

            return services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                // default is true
                options.RequireHttpsMetadata = false;
                // default is true, 将 jwtToken 保存到当前的 HttpContext, 以通过 await HttpContext.GetTokenAsync("Bearer","access_token"); 获取它
                options.SaveToken = true;
                options.TokenValidationParameters = tokenValidationParams;
            });
        }
    }
}