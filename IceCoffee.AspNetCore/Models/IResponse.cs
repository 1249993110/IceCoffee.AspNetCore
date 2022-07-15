using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Text.Json.Serialization;

namespace IceCoffee.AspNetCore.Models
{
    /// <summary>
    /// 响应
    /// </summary>
    internal interface IResponse : IConvertToActionResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        [JsonPropertyName("status")]
        HttpStatus Status { get; set; }
    }
}