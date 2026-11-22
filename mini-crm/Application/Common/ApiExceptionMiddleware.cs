using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using mini_crm.Application.Common;

namespace mini_crm.Application.Common;

public sealed class ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ArgumentOutOfRangeException exception)
        {
            await WriteProblemAsync(context, 400, "Bad Request", "One or more query or request values are invalid.", "validation_error",
                new Dictionary<string, string[]> { [exception.ParamName ?? "request"] = ["The supplied value is outside the allowed range."] });
        }
        catch (ArgumentException exception)
        {
            await WriteProblemAsync(context, 400, "Bad Request", exception.Message, "validation_error");
        }
        catch (KeyNotFoundException)
        {
            await WriteProblemAsync(context, 404, "Not Found", "The requested resource was not found.", "not_found");
        }
        catch (InvalidOperationException exception)
        {
            await WriteProblemAsync(context, 409, "Conflict", exception.Message, "conflict");
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled API exception");
            await WriteProblemAsync(context, 500, "Internal Server Error", "An unexpected error occurred.", "internal_error");
        }

        static async Task WriteProblemAsync(HttpContext context, int status, string title, string detail, string code,
            IReadOnlyDictionary<string, string[]>? errors = null)
        {
            context.Response.StatusCode = status;
            context.Response.ContentType = "application/problem+json";
            var problem = new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = detail,
                Instance = context.Request.Path
            };
            problem.Extensions["code"] = code;
            if (errors is not null)
                problem.Extensions["errors"] = errors;

            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
}
