using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using UsersService.DataAccess.Exceptions;

namespace UsersService.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
            => _next = next;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var response = httpContext.Response;
                response.ContentType = "application/json";

                response.StatusCode = ex switch
                {
                    UserInfoNotFoundException       => (int)HttpStatusCode.NotFound,
                    ArgumentNullException           => (int)HttpStatusCode.BadRequest,
                    Exception                       => (int)HttpStatusCode.InternalServerError,
                };

                var result = JsonSerializer.Serialize(new { message = ex?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
