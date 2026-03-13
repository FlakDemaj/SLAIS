namespace Application.Utils.MediatR.Interfaces;

public interface IMediatR
{
    Task<TResponse> SendAsync<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default);
}