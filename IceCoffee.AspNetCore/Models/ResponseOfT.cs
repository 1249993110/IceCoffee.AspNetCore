using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using NJsonSchema.Annotations;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace IceCoffee.AspNetCore.Models
{
    /// <summary>
    /// 泛型响应
    /// </summary>
    public class Response<TData> : IResponse
    {
        /// <summary>
        /// 状态码
        /// </summary>
        [JsonPropertyName("status")]
        public HttpStatus Status { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        [JsonPropertyName("data")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TData? Data { get; set; }

        /// <summary>
        /// 错误
        /// </summary>
        [JsonPropertyName("error")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonSchemaIgnore]
        public Error? Error { get; set; }

        /// <summary>
        /// Implicitly converts the specified <paramref name="response"/> to an <see cref="Response{TData}"/>.
        /// </summary>
        /// <param name="response">The value to convert.</param>
        public static implicit operator Response<TData>(Response response)
        {
            return new Response<TData>()
            {
                Status = response.Status,
                Error = response.Error
            };
        }

        [DebuggerStepThrough]
        IActionResult IConvertToActionResult.Convert()
        {
            return new JsonResult(this)
            {
                StatusCode = (int)Status
            };
        }
    }
}