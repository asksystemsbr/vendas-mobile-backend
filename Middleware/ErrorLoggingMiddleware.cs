using ControlStoreAPI.Services.Interface;

namespace ControlStoreAPI.Middleware
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILoggerService loggerService)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Registra o erro usando o serviço de log
                var routeData = context.GetRouteData();
                var controller = routeData?.Values["controller"];
                var action = routeData?.Values["action"];
                var user = context.User;

                await loggerService.LogError("HTTP Request", $"{controller}/{action}", user, ex);

                // Rejeita a exceção para ser tratada pelo middleware de tratamento de erro padrão do ASP.NET Core
                throw;
            }
        }
    }
}
