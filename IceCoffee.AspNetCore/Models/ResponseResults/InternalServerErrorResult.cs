using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace IceCoffee.AspNetCore.Models.ResponseResults
{
    public class InternalServerErrorResult
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
