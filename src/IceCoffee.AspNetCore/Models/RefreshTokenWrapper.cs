namespace IceCoffee.AspNetCore.Models
{
    /// <summary>
    /// RefreshTokenWrapper
    /// </summary>
    public class RefreshTokenWrapper
    {
        /// <summary>
        /// 刷新令牌
        /// </summary>
        [Required]
        public string RefreshToken { get; set; } = null!;
    }
}
