using Newtonsoft.Json;
using Version.KerwaException;

namespace Version.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");

                // Return a consistent error response
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var errorResponse = new ErrorResponse
                {
                    Message = "An unexpected error occurred. Please try again later."
                };

                await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
            }
        }
    }
}
