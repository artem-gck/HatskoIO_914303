using Microsoft.EntityFrameworkCore;
using StructureService.Domain.Exceptions;
using System.Net;

namespace StructureService.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> exceptionLogger)
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

                await response.WriteAsJsonAsync(new { message = ex?.Message });
            }
        }
    }
}
