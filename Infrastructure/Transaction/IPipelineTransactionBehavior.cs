namespace Infrastructure.Transaction;

public interface IPipelineTransactionBehavior<TRequest, TResponse>
{
    Task<TResponse> HandleAsync(
        TRequest request,
        Func<Task<TResponse>> next,
        CancellationToken cancellationToken);
}
