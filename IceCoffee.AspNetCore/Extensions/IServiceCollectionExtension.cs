using IceCoffee.AspNetCore.Authorization;
using IceCoffee.AspNetCore.Models;
using IceCoffee.AspNetCore.Options;
using IceCoffee.AspNetCore.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace IceCoffee.AspNetCore.Extensions
{
    public static class IServiceCollectionExtension
    {
        /// <summary>
        /// 添加 UserInfo 为 Scoped 到 IServiceCollection
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddUserInfo(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.TryAddScoped(services => new UserInfo(services.GetRequiredService<IHttpContextAccessor>().HttpContext?.User.Claims ?? Array.Empty<Claim>()));
            return services;
        }

        /// <summary>
        /// 添加 Jwt 认证策略到 IServiceCollection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="jwtOptionsSection"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddJwtAuthentication(this IServiceCollection services, IConfigurationSection jwtOptionsSection)
        {
            services.Configure<JwtOptions>(jwtOptionsSection);
            var jwtOptions = jwtOptionsSection.Get<JwtOptions>();

            return AddJwtAuthentication(services, jwtOptions);
        }

        /// <summary>
        /// 添加 Jwt 认证策略到 IServiceCollection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="jwtOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddJwtAuthentication(IServiceCollection services, JwtOptions jwtOptions)
        {
            if (jwtOptions.SecretKey == null)
            {
                throw new Exception("The SecretKey can not be null");
            }

            var key = Encoding.UTF8.GetBytes(jwtOptions.SecretKey);

            var tokenValidationParams = new TokenValidationParameters()
            {
                // 是否校验安全令牌
                ValidateIssuerSigningKey = jwtOptions.ValidateIssuerSigningKey,
                // 是否校验过期时间
                ValidateLifetime = jwtOptions.ValidateLifetime,
                // 是否校验颁发者
                ValidateIssuer = jwtOptions.ValidateIssuer,
                // 是否校验被颁发者
                ValidateAudience = jwtOptions.ValidateAudience,
                // 安全令牌
                IssuerSigningKey = new SymmetricSecurityKey(key),
                // 颁发者
                ValidIssuer = jwtOptions.ValidIssuer,
                // 被颁发者
                ValidAudience = jwtOptions.ValidAudience,
                /* 缓冲过期时间, 总的有效时间等于这个时间加上jwt的过期时间
                   默认允许 300s 的时间偏移量, 设置为0即可                  */
                ClockSkew = TimeSpan.FromSeconds(jwtOptions.ClockSkew),
                // 指示令牌是否必须具有“到期”值
                RequireExpirationTime = jwtOptions.RequireExpirationTime
            };

            services.AddSingleton(tokenValidationParams);

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
                options.SaveToken = false;
                options.TokenValidationParameters = tokenValidationParams;
            });
        }

        /// <summary>
        /// 添加 Jwt 授权策略到 IServiceCollection
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJwtAuthorization(this IServiceCollection services)
        {
            return services.AddJwtAuthorization(new PermissionRequirement());
        }

        /// <summary>
        /// 添加 Jwt 授权策略到 IServiceCollection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="permissionRequirement"></param>
        /// <returns></returns>
        public static IServiceCollection AddJwtAuthorization(this IServiceCollection services, PermissionRequirement permissionRequirement)
        {
            // 添加授权处理器（默认添加微信和PC）, 这里不能使用 TryAdd, 否则只会添加一个 IAuthorizationHandler
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IAuthorizationHandler, AuthorizationHandler>());

            services.AddSingleton(permissionRequirement);

            // 添加授权策略服务
            services.AddAuthorization(options =>
            {
                #region 备忘

                // 只是设置被 Authorize 特性标记的 Controller 或 Action 的默认认证计划
                // options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

                //// 使用 （IAuthorizationRequirement 和 AuthorizationHandler）或 policy.AddAuthenticationSchemes 验证, 
                //// 此方法仅可实现让被自定义特性标记的 Controller 或 Action 能绕过 FallbackPolicy, 对其进行直接授权, 而无法默认覆盖所有控制器

                #endregion 备忘

                // https://blog.csdn.net/sD7O95O/article/details/105382881
                // InvokeHandlersAfterFailure 为 true 的情况下（默认为 true ）, 所有注册了的 AuthorizationHandler 都会被执行
                options.InvokeHandlersAfterFailure = false;

                // 如果资源具有任何 IAuthorizeData 实例, 则将对它们进行评估, 而不是回退策略
                options.FallbackPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .AddRequirements(permissionRequirement)
                    .Build();
            });

            return services;
        }

        /// <summary>
        /// 添加默认邮件服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureSmtpOptions"></param>
        public static void AddEmailService(this IServiceCollection services, Action<SmtpOptions> configureSmtpOptions)
        {
            services.Configure(configureSmtpOptions);
            services.TryAddSingleton<EmailService>();
        }

        /// <summary>
        /// 添加默认邮件服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="smtpOptionsSection"></param>
        public static void AddEmailService(this IServiceCollection services, IConfigurationSection smtpOptionsSection)
        {
            services.Configure<SmtpOptions>(smtpOptionsSection);
            services.TryAddSingleton<EmailService>();
        }
    }
}