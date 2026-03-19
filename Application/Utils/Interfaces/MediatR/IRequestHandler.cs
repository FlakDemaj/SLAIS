using Application.Utils.Interfaces.Mediator;

namespace Application.Utils.Mediator.Interfaces;

public interface IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
