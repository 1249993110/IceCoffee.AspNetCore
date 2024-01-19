using IceCoffee.AspNetCore.Models;
using IceCoffee.AspNetCore.Options;
using IceCoffee.AspNetCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace IceCoffee.AspNetCore.Extensions
{
    /// <summary>
    /// IServiceCollection Extension
    /// </summary>
    public static class IServiceCollectionExtension
    {
        /// <summary>
        /// 添加 <see cref="UserInfo"/> 为 Scoped 到 IServiceCollection
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
        /// 添加 TUserInfo 为 Scoped 到 IServiceCollection
        /// </summary>
        /// <typeparam name="TUserInfo"></typeparam>
        /// <param name="services"></param>
        /// <param name="implementationFactory"></param>
        /// <returns></returns>
        public static IServiceCollection AddUserInfo<TUserInfo>(this IServiceCollection services, Func<IServiceProvider, TUserInfo> implementationFactory) where TUserInfo : UserInfo
        {
            services.AddHttpContextAccessor();
            services.TryAddScoped(implementationFactory);

            return services;
        }

        ///// <summary>
        ///// 添加联合的 Cookie 与 ApiKey 区域授权策略服务到 IServiceCollection
        ///// </summary>
        ///// <param name="services"></param>
        ///// <returns></returns>
        //public static IServiceCollection AddAreaAuthorization(this IServiceCollection services)
        //{
        //    // 添加授权策略服务
        //    services.AddAuthorization(options =>
        //    {
        //        #region 备忘

        //        // 只是设置被 Authorize 特性标记的 Controller 或 Action 的默认认证计划
        //        // options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

        //        //// 使用 （IAuthorizationRequirement 和 AuthorizationHandler）或 policy.AddAuthenticationSchemes 验证,
        //        //// 此方法仅可实现让被自定义特性标记的 Controller 或 Action 能绕过 FallbackPolicy, 对其进行直接授权, 而无法默认覆盖所有控制器

        //        #endregion 备忘

        //        // https://blog.csdn.net/sD7O95O/article/details/105382881
        //        // InvokeHandlersAfterFailure 为 true 的情况下（默认为 true ）, 所有注册了的 AuthorizationHandler 都会被执行
        //        // options.InvokeHandlersAfterFailure = false;

        //        // 靠后的 Scheme 将覆盖前面的 Identity
        //        var policy = new AuthorizationPolicyBuilder(
        //            AuthenticationSchemes.ApiKeyAuthenticationScheme,
        //            CookieAuthenticationDefaults.AuthenticationScheme)
        //            .RequireAuthenticatedUser() // DenyAnonymousAuthorizationRequirement
        //            .AddRequirements(new UriAuthorizationRequirement())
        //            .Build();
        //        options.DefaultPolicy = policy;
        //        // 如果资源具有任何 IAuthorizeData 实例, 则将对它们进行评估, 而不是回退策略, authenticationSchemes 认证方案按反序进行
        //        options.FallbackPolicy = policy;
        //    });

        //    return services;

        //    // 不需要将 AreaAuthorizationRequirement 注册为服务
        //    // 因为在 PassThroughAuthorizationHandler 中会自动遍历实现了 IAuthorizationHandler 的 IAuthorizationRequirement
        //    // services.TryAddEnumerable(ServiceDescriptor.Singleton<IAuthorizationHandler, AreaAuthorizationRequirement>());
        //    // services.AddSingleton<IAuthorizationHandler, AreaAuthorizationRequirement>();
        //}

        /// <summary>
        /// 添加邮件服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        public static void AddEmailService(this IServiceCollection services, Action<SmtpOptions> configure)
        {
            services.AddOptions<SmtpOptions>().Configure(configure);
            services.TryAddTransient<EmailService>();
        }

        /// <summary>
        /// 添加邮件服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurationSectionPath"></param>
        public static void AddEmailService(this IServiceCollection services, string configurationSectionPath)
        {
            services.AddOptions<SmtpOptions>()
                .BindConfiguration(configurationSectionPath)
                .ValidateDataAnnotations()
                .ValidateOnStart(); ;
            services.TryAddTransient<EmailService>();
        }

        /// <summary>
        /// 添加阿里云短信服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        public static void AddAliCloudSmsService(this IServiceCollection services, Action<AliCloudSmsOptions> configure)
        {
            services.AddOptions<AliCloudSmsOptions>().Configure(configure);
            services.TryAddTransient<AliCloudSmsService>();
        }

        /// <summary>
        /// 添加阿里云短信服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurationSectionPath"></param>
        public static void AddAliCloudSmsService(this IServiceCollection services, string configurationSectionPath)
        {
            services.AddOptions<AliCloudSmsOptions>()
                .BindConfiguration(configurationSectionPath)
                .ValidateDataAnnotations()
                .ValidateOnStart(); ;
            services.TryAddTransient<AliCloudSmsService>();
        }
    }
}