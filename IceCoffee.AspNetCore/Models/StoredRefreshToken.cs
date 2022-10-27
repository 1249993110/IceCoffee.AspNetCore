namespace IceCoffee.AspNetCore.Models
{
    /// <summary>
    /// Stored Refresh Token
    /// </summary>
    public class StoredRefreshToken
    {
        /// <summary>
        /// 使用 JwtId 映射到对应的 token
        /// </summary>
        public string? JwtId { get; set; }

        /// <summary>
        /// 是否出于安全原因已将其撤销
        /// </summary>
        public bool IsRevorked { get; set; }

        /// <summary>
        /// Refresh Token 的生命周期很长, 可以长达数月。注意一个Refresh Token只能被用来刷新一次
        /// </summary>
        public DateTime ExpiryDate { get; set; }
    }
}