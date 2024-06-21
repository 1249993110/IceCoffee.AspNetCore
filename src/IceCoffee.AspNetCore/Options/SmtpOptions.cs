using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IceCoffee.AspNetCore.Options
{
    /// <summary>
    /// SmtpOptions
    /// </summary>
    public class SmtpOptions
    {
        /// <summary>
        /// Host
        /// </summary>
        [Required]
        public string Host { get; set; } = null!;

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 启用SSL
        /// </summary>
        [DefaultValue(true)]
        public bool EnableSsl { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(false)]
        public bool UseDefaultCredentials { get; set; } = false;

        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserName { get; set; } = null!;

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; } = null!;
    }
}