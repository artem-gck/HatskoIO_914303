namespace UsersService.Middlewares
{
    /// <summary>
    /// Extension class for add middleware.
    /// </summary>
    public static class ExceptionHandlerMiddlewareExtensions
    {
        /// <summary>
        /// Configures the custom exception middleware.
        /// </summary>
        /// <param name="app">The application.</param>
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
