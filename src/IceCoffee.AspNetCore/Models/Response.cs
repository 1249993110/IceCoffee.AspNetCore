using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using NJsonSchema.Annotations;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace IceCoffee.AspNetCore.Models
{
    /// <summary>
    /// 基础响应
    /// </summary>
    public class Response : IResponse
    {
        /// <summary>
        /// 状态码
        /// </summary>
        [JsonPropertyName("status")]
        [DefaultValue((int)HttpStatus.BadRequest)]
        public HttpStatus Status { get; set; }

        /// <summary>
        /// 错误
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
}