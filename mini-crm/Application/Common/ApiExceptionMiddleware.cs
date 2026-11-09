using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace mini_crm.Application.Common;

public sealed class ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled API exception");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";
            var problem = new ProblemDetails
            {
                Status = context.Response.StatusCode,
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred."
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
}
