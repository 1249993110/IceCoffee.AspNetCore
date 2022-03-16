using Microsoft.AspNetCore.Authorization;
using System.ComponentModel;

namespace IceCoffee.AspNetCore.Authorization
{
    /// <summary>
    /// 许可授权要求
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 要求 HttpMethods 授权
        /// </summary>
        [DefaultValue(false)]
        public bool RequireHttpMethods { get; set; }
    }
}