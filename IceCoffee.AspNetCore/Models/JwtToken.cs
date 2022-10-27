using System.Text.Json.Serialization;

namespace IceCoffee.AspNetCore.Models
{
    public class JwtToken
    {
        /// <summary>
        /// jwtId
        /// </summary>
        [JsonIgnore]
        public string? Id { get; set; }

        /// <summary>
        /// 访问令牌
        /// </summary>
        public string? AccessToken { get; set; }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// 访问令牌到期时间
        /// </summary>
        public DateTime AccessTokenExpiryDate { get; set; }

        /// <summary>
        /// 刷新令牌到期时间
        /// </summary>
        public DateTime RefreshTokenExpiryDate { get; set; }
    }
}