namespace IceCoffee.AspNetCore.Models
{
    /// <summary>
    /// ShortMessageLogin
    /// </summary>
    public class ShortMessageLogin
    {
        /// <summary>
        /// 电话号码
        /// </summary>
        [Required]
        [RegularExpression("^(1)\\d{10}$")]
        public string PhoneNumber { get; set; } = null!;

        /// <summary>
        /// Base64编码的密码
        /// </summary>
        [Required]
        public string Code { get; set; } = null!;

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
