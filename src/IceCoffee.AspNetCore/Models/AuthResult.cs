namespace IceCoffee.AspNetCore.Models
{
    /// <summary>
    /// 认证结果
    /// </summary>
    public class AuthResult
    {
        /// <summary>
        /// jwtId
        /// </summary>
        [JsonIgnore]
        public string JwtId { get; set; } = null!;

        /// <summary>
        /// 访问令牌
        /// </summary>
        public string AccessToken { get; set; } = null!;

        /// <summary>
        /// 刷新令牌
        /// </summary>
        public string RefreshToken { get; set; } = null!;

        /// <summary>
        /// 访问令牌到期时间
        /// </summary>
        public DateTime AccessTokenExpiresAt { get; set; }

        /// <summary>
        /// 刷新令牌到期时间
        /// </summary>
        public DateTime RefreshTokenExpiresAt { get; set; }

        /// <summary>
        /// 要求用户更改密码
        /// </summary>
        public bool RequirePasswordChange { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public IEnumerable<string>? RoleNames { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public IEnumerable<Tag>? Tags { get; set; }
    }
}
