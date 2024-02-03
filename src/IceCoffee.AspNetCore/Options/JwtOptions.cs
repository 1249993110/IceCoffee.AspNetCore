using System.ComponentModel.DataAnnotations;

namespace IceCoffee.AspNetCore.Options
{
    /// <summary>
    /// JwtOptions
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// 安全令牌
        /// </summary>
        [Required]
        public string SecretKey { get; set; } = null!;

        /// <summary>
        /// 缓冲过期时间, 总的有效时间等于这个时间加上jwt的过期时间, 单位: 秒, 默认60秒
        /// </summary>
        public int ClockSkew { get; set; } = 60;

        /// <summary>
        /// 访问令牌过期时长, 单位: 秒, 默认10分钟
        /// </summary>
        public int AccessTokenExpires { get; set; } = 600;

        /// <summary>
        /// 刷新令牌过期时长, 单位: 秒, 默认为一周
        /// </summary>
        public int RefreshTokenExpires { get; set; } = 604800;

        /// <summary>
        /// 是否校验被颁发者
        /// </summary>
        public bool ValidateAudience { get; set; } = true;

        /// <summary>
        /// 被颁发者
        /// </summary>
        public string? ValidAudience { get; set; }
    }
}