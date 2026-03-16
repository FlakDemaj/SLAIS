namespace Application.Utils.MediatR.Interfaces;

public interface IMediatR
{
    Task<TResponse> SendAsync<TResponse>(
        IRequest<TResponse> response,
        CancellationToken cancellationToken = default);
}