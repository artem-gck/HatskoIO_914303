using SignatureService.DataAccess.DataBase.Exceptiions;
using SignatureService.DataAccess.Http.Exceptions;
using System.Net;

namespace SignatureServiceApi.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

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
                    DocumentNotFoundException => (int)HttpStatusCode.NotFound,
                    Exception => (int)HttpStatusCode.InternalServerError,
                };

                await response.WriteAsJsonAsync(new { message = ex?.Message });
            }
        }
    }
}
