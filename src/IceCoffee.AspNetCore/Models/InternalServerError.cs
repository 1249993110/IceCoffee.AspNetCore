namespace IceCoffee.AspNetCore.Models
{
    public class InternalServerError
    {
        /// <summary>
        /// 请求Id
        /// </summary>
        public string? TraceId { get; set; }

        public string? Message { get; set; }
    }
}