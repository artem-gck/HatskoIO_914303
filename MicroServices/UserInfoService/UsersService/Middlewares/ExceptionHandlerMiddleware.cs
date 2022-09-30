using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using UsersService.DataAccess.Exceptions;

namespace UsersService.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _exceptionLogger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> exceptionLogger)
        {
            _next = next;
            _exceptionLogger = exceptionLogger ?? throw new ArgumentNullException(nameof(exceptionLogger));
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _exceptionLogger.LogWarning("Exception in ExceptionHandlerMiddleware with text \"{message}\"", ex.Message);

                var response = httpContext.Response;

                response.StatusCode = ex switch
                {
                    UserInfoNotFoundException       => (int)HttpStatusCode.NotFound,
                    Exception                       => (int)HttpStatusCode.InternalServerError,
                };

                await response.WriteAsJsonAsync(new { message = ex?.Message });
            }
        }
    }
}
