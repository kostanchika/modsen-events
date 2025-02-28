using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text.Json;

namespace EventsAPI.Middlewares
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
            catch (ValidationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await WriteJsonErrorResponse(context, ex.Errors);
            }
            catch (SecurityTokenException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await WriteJsonErrorResponse(context, ex.Message);
            }
            catch (NullReferenceException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await WriteJsonErrorResponse(context, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                await WriteJsonErrorResponse(context, ex.Message);
            }
            catch (OperationCanceledException ex)
            {
                context.Response.StatusCode = 499;
                await WriteJsonErrorResponse(context, "Подключение было разорвано клиентом");
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await WriteJsonErrorResponse(context, "Произошла непредвиденная ошибка. Пожалуйста, попробуйте позже.");
            }
        }
        
        private static Task WriteJsonErrorResponse(HttpContext context, IEnumerable<FluentValidation.Results.ValidationFailure> errors)
        {
            var errorResponse = new
            {
                Errors = errors.Select(e => new { e.PropertyName, e.ErrorMessage }),
                StatusCode = context.Response.StatusCode,
                Timestamp = DateTime.UtcNow
            };

            var jsonResponse = JsonSerializer.Serialize(errorResponse);
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(jsonResponse);
        }

        private static Task WriteJsonErrorResponse(HttpContext context, string message)
        {
            var errorResponse = new
            {
                Errors = new List<FluentValidation.Results.ValidationFailure>() { new("error", message) },
                StatusCode = context.Response.StatusCode,
                Timestamp = DateTime.UtcNow
            };

            var jsonResponse = JsonSerializer.Serialize(errorResponse);
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
