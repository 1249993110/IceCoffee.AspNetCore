using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace IceCoffee.AspNetCore.Options
{
    public class JwtOptions
    {
        /// <summary>
        /// 是否校验安全令牌
        /// </summary>
        [DefaultValue(true)]
        public bool ValidateIssuerSigningKey { get; set; } = true;

        /// <summary>
        /// 是否校验过期时间
        /// </summary>
        [DefaultValue(true)]
        public bool ValidateLifetime { get; set; } = true;

        /// <summary>
        /// 是否校验颁发者
        /// </summary>
        [DefaultValue(true)]
        public bool ValidateIssuer { get; set; } = true;

        /// <summary>
        /// 是否校验被颁发者
        /// </summary>
        [DefaultValue(true)]
        public bool ValidateAudience { get; set; } = true;

        /// <summary>
        /// 安全令牌
        /// </summary>
        public string? SecretKey { get; set; }

        /// <summary>
        /// 颁发者
        /// </summary>
        public string? ValidIssuer { get; set; }

        /// <summary>
        /// 被颁发者
        /// </summary>
        public string? ValidAudience { get; set; }

        /// <summary>
        /// 缓冲过期时间，总的有效时间等于这个时间加上jwt的过期时间
        /// 默认允许 300s 的时间偏移量，设置为0即可
        /// </summary>
        [DefaultValue(0)]
        public int ClockSkew { get; set; }

        /// <summary>
        /// 指示令牌是否必须具有“到期”值
        /// </summary>
        [DefaultValue(true)]
        public bool RequireExpirationTime { get; set; } = true;
    }
}
