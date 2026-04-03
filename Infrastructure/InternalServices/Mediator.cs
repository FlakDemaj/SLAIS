using Application.Common.Authentication;
using Application.Utils.Interfaces.Mediator;
using Application.Utils.Logger;
using Application.Utils.Mediator.Interfaces;

using Infrastructure.Pipelines;
using Infrastructure.Pipelines.Transaction;

using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.InternalServices;

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    private readonly ISlaisLogger<Mediator> _logger;

    public Mediator(
        IServiceProvider serviceProvider,
        ISlaisLogger<Mediator> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task<TResponse> SendAsync<TResponse>(
        IRequest<TResponse> request,
        IAuthentication? authentication = null,
        CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        var handlerType = typeof(IRequestHandler<,>)
            .MakeGenericType(requestType, typeof(TResponse));

        var handler = _serviceProvider.GetRequiredService(handlerType);

        var method = handlerType.GetMethod("HandleAsync");

        if (method == null)
        {
            _logger.LogError($"Handler method not found for type {handlerType}", null);
        }

        var pipelineType = typeof(IPipelineBehavior<,>)
            .MakeGenericType(requestType, typeof(TResponse));

        var pipeline = _serviceProvider.GetRequiredService(pipelineType);

        var pipelineMethod = pipelineType.GetMethod("HandleAsync");

        return await (Task<TResponse>)pipelineMethod?
            .Invoke(pipeline, [request, (Func<Task<TResponse>?>)Next, cancellationToken])!;

        Task<TResponse>? Next() => (Task<TResponse>)method?.Invoke(handler, [request, authentication, cancellationToken])!;
    }
}
