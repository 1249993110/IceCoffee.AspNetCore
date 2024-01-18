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
        /// 缓冲过期时间, 总的有效时间等于这个时间加上jwt的过期时间
        /// </summary>
        public int ClockSkew { get; set; }
    }
}