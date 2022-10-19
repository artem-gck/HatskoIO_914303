namespace CompanyManagementServiceApi.Middlewares
{
    public static class ExceptionHandlerMiddlewareExtension
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
