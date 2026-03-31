using Application.Common.Authentication;
using Application.Utils.Interfaces.Mediator;

using Domain.Common;

namespace Application.Utils.Mediator.Interfaces;

public interface IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> HandleAsync(
        TRequest request,
        IAuthentication? authentication = null,
        CancellationToken cancellationToken = default);
}
