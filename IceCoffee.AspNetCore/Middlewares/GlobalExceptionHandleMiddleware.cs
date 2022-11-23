using IceCoffee.AspNetCore.Extensions;
using IceCoffee.AspNetCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace IceCoffee.AspNetCore.Middlewares
{
    /// <summary>
    /// 全局异常处理中间件
    /// 处理mvc异常过滤器中发生的异常 以及其他非mvc层中的异常
    /// </summary>
    public class GlobalExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandleMiddleware> _logger;

        public GlobalExceptionHandleMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandleMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        private static void GetInnerMessage(Exception ex, List<string> details)
        {
            if (ex.InnerException != null)
            {
                details.Add(ex.InnerException.Message);
                GetInnerMessage(ex.InnerException, details);
            }
        }

        private static IEnumerable<string> GetDetails(Exception ex)
        {
            var details = new List<string>();
            GetInnerMessage(ex, details);
            return details;
        }

        /// <summary>
        /// AspNetCore 的管道执行至 ExceptionHandlerMiddleware 时, 捕获其他中间件异常
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = StatusCodes.Status500InternalServerError;

                string requestId = context.GetRequestId();

#pragma warning disable CS8601 // 引用类型赋值可能为 null。
                var result = new Response()
                {
                    Status = HttpStatus.InternalServerError,
                    Error = new InternalServerError() 
                    {
                        RequestId = requestId,
                        Message = ex.Message,
                        Details = GetDetails(ex)
                    }
                };
#pragma warning restore CS8601 // 引用类型赋值可能为 null。

                await JsonSerializer.SerializeAsync(response.Body, result);

                var path = context.Request.Path;

                _logger.LogError(ex, 
                    "The exception is caught in the global exception handling middleware, requestId: {requestId}, path: {path}"
                    , requestId, path);
            }
        }
    }
}