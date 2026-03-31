using Application.Common.Authentication;

namespace Application.Utils.Interfaces.Mediator;

public interface IMediator
{
    Task<TResponse> SendAsync<TResponse>(
        IRequest<TResponse> request,
        IAuthentication? authentication = null,
        CancellationToken cancellationToken = default);
}
