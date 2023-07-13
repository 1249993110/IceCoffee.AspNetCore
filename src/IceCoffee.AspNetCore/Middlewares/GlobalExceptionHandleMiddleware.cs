using IceCoffee.AspNetCore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
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

        /// <summary>
        /// AspNetCore 的管道执行至 ExceptionHandlerMiddleware 时, 捕获其他中间件异常
        /// </summary>
        /// <param name="context"></param>
        /// <param name="options"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public async Task Invoke(HttpContext context, IOptions<Microsoft.AspNetCore.Mvc.JsonOptions> options, IWebHostEnvironment env)
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

                string requestId = context.TraceIdentifier;
                var model = new InternalServerError()
                {
                    TraceId = requestId,
                    Message = env.IsDevelopment() ? ex.Message : "Internal server error",
                };

                await JsonSerializer.SerializeAsync(response.Body, model, options.Value.JsonSerializerOptions);

                var path = context.Request.Path;

                _logger.LogError(
                    ex,
                    "The exception is caught in the global exception handling middleware, requestId: {requestId}, path: {path}", 
                    requestId,
                    path);
            }
        }
    }
}