using System.Reflection;

using Application.Utils.Logger;
using Application.Utils.MediatR.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace Application.Utils.MediatR;

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
        Type requestType = request.GetType();

        Type handlerType = typeof(IRequestHandler<,>)
            .MakeGenericType(requestType, typeof(TResponse));

        var handler = _serviceProvider.GetRequiredService(handlerType);

        if (handler == null)
        {
            _logger.LogError($"Handler not found for type {handlerType}", null);
        }

        MethodInfo? method = handlerType.GetMethod("HandleAsync");

        if (method == null)
        {
            _logger.LogError($"Handler method not found for type {handlerType}", null);
        }

        Type pipelineType = typeof(IPipelineTransactionBehavior<,>)
            .MakeGenericType(requestType, typeof(TResponse));

        var pipeline = _serviceProvider.GetRequiredService(pipelineType);

        MethodInfo? pipelineMethod = pipelineType.GetMethod("HandleAsync");

        return await (Task<TResponse>)pipelineMethod?.Invoke(pipeline, [request, (Func<Task<TResponse>?>)Next, cancellationToken])!;

        Task<TResponse>? Next() => (Task<TResponse>)method?.Invoke(handler, [request, cancellationToken])!;
    }
}
