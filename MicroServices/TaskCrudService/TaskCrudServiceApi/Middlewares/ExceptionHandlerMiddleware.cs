using Microsoft.EntityFrameworkCore;
using NLog;
using System.Net;
using TaskCrudService.Domain.Exceptions;
using ILogger = NLog.ILogger;

namespace TaskCrudService.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var response = httpContext.Response;

                response.StatusCode = ex switch
                {
                    NotFoundException => (int)HttpStatusCode.NotFound,
                    DbUpdateException => (int)HttpStatusCode.Conflict,
                    Exception => (int)HttpStatusCode.InternalServerError,
                };

                _logger.Warn(ex, "Exception in ExceptionHandlerMiddleware");

                await response.WriteAsJsonAsync(new { message = ex?.Message });
            }
        }
    }
}
