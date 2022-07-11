using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace IceCoffee.AspNetCore.Models
{
    /// <summary>
    /// 响应基类
    /// </summary>
    public class Response : IConvertToActionResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        [JsonPropertyName("status")]
        public HttpStatus Status { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [JsonPropertyName("error")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Error? Error { get; set; }

        [DebuggerStepThrough]
        IActionResult IConvertToActionResult.Convert()
        {
            return new JsonResult(this)
            {
                StatusCode = (int)Status
            };
        }
    }

    /// <summary>
    /// 泛型响应
    /// </summary>
    public class Response<TData> : Response
    {
        /// <summary>
        /// 数据
        /// </summary>
        [JsonPropertyName("data")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TData? Data { get; set; }
    }
}