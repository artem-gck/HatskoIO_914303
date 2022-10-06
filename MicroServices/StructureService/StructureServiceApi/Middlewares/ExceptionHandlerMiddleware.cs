using Microsoft.EntityFrameworkCore;
using StructureService.Domain.Exceptions;
using System.Net;

namespace StructureServiceApi.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

                _logger.LogWarning(ex, "Exception in ExceptionHandlerMiddleware");

                await response.WriteAsJsonAsync(new { message = ex?.Message });
            }
        }
    }
}
