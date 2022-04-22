using System.Text.Json.Serialization;

namespace IceCoffee.AspNetCore.Models
{
    public class InternalServerError
    {
        /// <summary>
        /// 请求Id
        /// </summary>
        [JsonPropertyName("requestId")]
        public string? RequestId { get; set; }

        /// <summary>
        /// Ip地址
        /// </summary>
        [JsonPropertyName("ipAddress")]
        public string? IpAddress { get; set; }
    }
}