namespace IceCoffee.AspNetCore.Models
{
    /// <summary>
    /// 控制器中的自定义状态代码
    /// </summary>
    public enum CustomStatusCode : short
    {
        /// <summary>
        /// 成功
        /// </summary>
        OK = 200,

        /// <summary>
        /// 错误的请求
        /// </summary>
        BadRequest = 400,

        /// <summary>
        /// 未授权
        /// </summary>
        Forbidden = 403,

        /// <summary>
        /// 内部服务器错误
        /// </summary>
        InternalServerError = 500
    }
}