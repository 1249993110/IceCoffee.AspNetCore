namespace IceCoffee.AspNetCore.Models
{
    /// <summary>
    /// UserLogin
    /// </summary>
    public class UserLogin
    {
        /// <summary>
        /// 登录名称, 可以是用户名、电子邮箱、或者手机号码
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
        [Required]
        public Guid? AppId { get; set; } = null!;
    }
}
