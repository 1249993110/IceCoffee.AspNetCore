namespace IceCoffee.AspNetCore.Models
{
    /// <summary>
    /// 内部服务器错误
    /// </summary>
    public class InternalServerError
    {
        /// <summary>
        /// 请求Id
        /// </summary>
        public string? TraceId { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string? Message { get; set; }
    }
}