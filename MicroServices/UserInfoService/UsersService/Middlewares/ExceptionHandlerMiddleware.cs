using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using UsersService.DataAccess.Exceptions;

namespace UsersService.Middlewares
{
    /// <summary>
    /// Middleware for exceptions handling.
    /// </summary>
    public class ExceptionHandlerMiddleware
    {
        /// <summary>
        /// The next.
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlerMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ExceptionHandlerMiddleware(RequestDelegate next)
            => _next = next;

        /// <summary>
        /// Invokes the asynchronous.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
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
