namespace mini_crm.Application.Abstractions;

using mini_crm.Application.Common;

public interface IRequestHandler<in TRequest, TResponse>
{
    Task<OperationResult<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
