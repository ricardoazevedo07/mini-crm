namespace mini_crm.Application.Common;

public sealed class OperationResult<T>
{
    private OperationResult(bool succeeded, T? value, ApplicationError? error)
    {
        Succeeded = succeeded;
        Value = value;
        Error = error;
    }

    public bool Succeeded { get; }
    public T? Value { get; }
    public ApplicationError? Error { get; }

    public static OperationResult<T> Success(T value) => new(true, value, null);
    public static OperationResult<T> Failure(ApplicationError error) => new(false, default, error);
}

public sealed record ApplicationError(string Code, string Message, int StatusCode = StatusCodes.Status400BadRequest, IReadOnlyDictionary<string, string[]>? Details = null);

public static class ApplicationErrors
{
    public static ApplicationError Validation(string message, IReadOnlyDictionary<string, string[]>? details = null) =>
        new("validation_error", message, StatusCodes.Status400BadRequest, details);

    public static ApplicationError NotFound(string message) =>
        new("not_found", message, StatusCodes.Status404NotFound);

    public static ApplicationError Conflict(string message) =>
        new("conflict", message, StatusCodes.Status409Conflict);
}
