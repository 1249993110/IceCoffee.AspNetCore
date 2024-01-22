namespace IceCoffee.AspNetCore.Models
{
    /// <summary>
    /// AccountNumberLogin
    /// </summary>
    public class AccountNumberLogin
    {
        /// <summary>
        /// 登录名称, 可以是用户名、电子邮箱、或者电话号码
        /// </summary>
        [Required]
        public string LoginName { get; set; } = null!;

        /// <summary>
        /// Base64编码的密码
        /// </summary>
        [Required]
        public string Password { get; set; } = null!;

        /// <summary>
        /// 应用Id
        /// </summary>
        public Guid AppId { get; set; }

        /// <summary>
        /// State
        /// </summary>
        public string? State { get; set; }
    }
}
