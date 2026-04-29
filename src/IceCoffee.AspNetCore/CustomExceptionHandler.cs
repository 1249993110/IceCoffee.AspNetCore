using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace IceCoffee.AspNetCore
{
    /// <summary>
    /// A global unhandled-exception handler that integrates with ASP.NET Core's
    /// <see cref="IExceptionHandler"/> pipeline. Registered via
    /// <c>services.AddExceptionHandler&lt;CustomExceptionHandler&gt;()</c>, it is invoked by
    /// <c>UseExceptionHandler()</c> before the fallback delegate, ensuring all unhandled exceptions
    /// from MVC controllers and minimal-API endpoints are logged and surfaced as RFC 7807
    /// <see cref="ProblemDetails"/> JSON responses.
    /// </summary>
    public class CustomExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<CustomExceptionHandler> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IProblemDetailsService _problemDetailsService;

        /// <summary>
        /// Initialises the handler with its required dependencies, all resolved from DI.
        /// </summary>
        /// <param name="logger">Used to emit a structured error log entry that includes the distributed trace ID.</param>
        /// <param name="env">Determines whether the full exception details are included in the response body.</param>
        /// <param name="problemDetailsService">Writes the <see cref="ProblemDetails"/> payload using the configured formatters.</param>
        public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger, IWebHostEnvironment env, IProblemDetailsService problemDetailsService)
        {
            _logger = logger;
            _env = env;
            _problemDetailsService = problemDetailsService;
        }

        /// <summary>
        /// Handles an unhandled exception by logging it and writing a 500
        /// <see cref="ProblemDetails"/> response. The <c>detail</c> field contains the full
        /// exception string only in the <c>Development</c> environment to avoid information
        /// disclosure in production.
        /// </summary>
        /// <param name="httpContext">The current HTTP context.</param>
        /// <param name="exception">The unhandled exception to process.</param>
        /// <param name="cancellationToken">A token that may signal the request has been aborted.</param>
        /// <returns>
        /// Always returns <see langword="true"/> to signal that the exception has been handled
        /// and no further handlers in the chain should be invoked.
        /// </returns>
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unexpected error occurred. TraceId: {TraceId}", Activity.Current?.Id ?? httpContext.TraceIdentifier);

            var problemDetails = new ProblemDetails()
            {
                Title = "An unexpected error occurred.",
                Status = StatusCodes.Status500InternalServerError,
                Instance = httpContext.Request.Path,
                Detail = _env.IsDevelopment() ? exception.ToString() : null
            };

            await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = problemDetails
            });

            return true;
        }
    }
}
