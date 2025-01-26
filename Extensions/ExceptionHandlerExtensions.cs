using ControlStoreAPI.Middleware;

namespace ControlStoreAPI.Extensions
{
    public static class ExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ErrorLoggingMiddleware>();
        }
    }
}
