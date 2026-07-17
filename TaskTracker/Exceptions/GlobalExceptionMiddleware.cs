using System.Net;
using System.Text.Json;

namespace TaskTracker.Exceptions
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Növbəti middleware və ya controller metodunu çağırırıq
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gözlənilməz bir xəta baş verdi: {Message}", ex.Message);

                //xetanin idaresi
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            //500 atiriq
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "Sistemdə daxili xəta baş verdi. Zəhmət olmasa bir az sonra yenidən yoxlayın.";

            // Xətanın növünə görə HTTP status kodunu dəyişirik
            if (exception is KeyNotFoundException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
            }
            else if (exception is ArgumentException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
            }

            context.Response.StatusCode = (int)statusCode;

            // Çölə qaytaracağımız standart xəta obyekti
            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = message,
                Detailed = exception.Message //mesaji gormek ucun 
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}