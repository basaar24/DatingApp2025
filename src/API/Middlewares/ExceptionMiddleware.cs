using System.Net;
using System.Text.Json;
using API.Exceptions;

namespace API.Middlewares;

public class ExceptionMiddleware(
    RequestDelegate next,
    ILogger<ExceptionMiddleware> logger,
    IHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            logger.LogError(ex, "{message}", ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var response = env.IsDevelopment() ?
                new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace) :
                new ApiException(context.Response.StatusCode, ex.Message, "The provided params are incorrect");

            var option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, option);

            await context.Response.WriteAsync(json);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{message}", ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = env.IsDevelopment() ?
                new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace) :
                new ApiException(context.Response.StatusCode, ex.Message, "Internal server error");

            var option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, option);

            await context.Response.WriteAsync(json);
        }
    }

}