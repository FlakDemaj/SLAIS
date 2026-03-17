namespace Application.Utils.Interfaces.MediatR;

public interface IMediatR
{
    Task<TResponse> SendAsync<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default);
}
