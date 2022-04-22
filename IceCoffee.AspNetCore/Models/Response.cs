using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace IceCoffee.AspNetCore.Models
{
    public interface IResponse : IConvertToActionResult
    {
    }

    /// <summary>
    /// 响应基类
    /// </summary>
    public abstract class ResponseBase : IResponse
    {
        /// <summary>
        /// 状态码
        /// </summary>
        [JsonPropertyName("code")]
        public CustomStatusCode Code { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [DebuggerStepThrough]
        IActionResult IConvertToActionResult.Convert()
        {
            return new JsonResult(this)
            {
                StatusCode = (int)Code
            };
        }
    }

    /// <summary>
    /// 响应
    /// </summary>
    public class Response : ResponseBase
    {
        /// <summary>
        /// 数据
        /// </summary>
        [JsonPropertyName("data")]
        public object? Data { get; set; }
    }

    /// <summary>
    /// 泛型响应
    /// </summary>
    public class Response<TData> : ResponseBase
    {
        /// <summary>
        /// 数据
        /// </summary>
        [JsonPropertyName("data")]
        public TData? Data { get; set; }
    }
}