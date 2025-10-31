using System.Net;
using System.Text.Json;
using YuGiOh.Domain.Exceptions;

namespace YuGiOh.WebAPI.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (APIException ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)ex.StatusCode;
                await context.Response.WriteAsync(JsonSerializer.Serialize(ex.ToProblemDetails()));
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    status = 500,
                    title = "Internal Server Error",
                    message = ex.Message,
                    timestamp = DateTime.UtcNow
                }));
            }
        }
    }
}

// app.UseMiddleware<ExceptionHandlingMiddleware>();
