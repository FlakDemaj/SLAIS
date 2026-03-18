using Application.Utils;
using Application.Utils.Interfaces.MediatR;
using Application.Utils.Logger;
using Application.Utils.MediatR.Interfaces;

using Infrastructure.Transaction;

using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.InternalServices;

public class MediatR : IMediatR
{
    private readonly IServiceProvider _serviceProvider;

    private readonly ISlaisLogger<MediatR> _logger;

    public MediatR(
        IServiceProvider serviceProvider,
        ISlaisLogger<MediatR> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        var handlerType = typeof(IRequestHandler<,>)
            .MakeGenericType(requestType, typeof(TResponse));

        var handler = _serviceProvider.GetRequiredService(handlerType);

        if (handler == null)
        {
            _logger.LogError($"Handler not found for type {handlerType}", null);
        }

        var method = handlerType.GetMethod("HandleAsync");

        if (method == null)
        {
            _logger.LogError($"Handler method not found for type {handlerType}", null);
        }

        var pipelineType = typeof(IPipelineTransactionBehavior<,>)
            .MakeGenericType(requestType, typeof(TResponse));

        var pipeline = _serviceProvider.GetRequiredService(pipelineType);

        var pipelineMethod = pipelineType.GetMethod("HandleAsync");

        return await (Task<TResponse>)pipelineMethod?.Invoke(pipeline, [request, (Func<Task<TResponse>?>)Next, cancellationToken])!;

        Task<TResponse>? Next() => (Task<TResponse>)method?.Invoke(handler, [request, cancellationToken])!;
    }
}
