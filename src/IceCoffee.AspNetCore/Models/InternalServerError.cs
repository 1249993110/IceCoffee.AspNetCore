using System.Text.Json.Serialization;

namespace IceCoffee.AspNetCore.Models
{
    public class InternalServerError : Error
    {
        /// <summary>
        /// 请求Id
        /// </summary>
        [JsonPropertyName("requestId")]
        public string? RequestId { get; set; }
    }
}