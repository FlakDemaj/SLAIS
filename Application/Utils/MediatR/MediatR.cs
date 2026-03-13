using Application.Utils.Logger;
using Application.Utils.MediatR.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Utils.MediatR;

public class MediatR : IMediatR
{
    private readonly IServiceProvider _serviceProvider;
    
    private readonly ISAISLogger _logger;

    public MediatR(
        IServiceProvider serviceProvider,
        ISAISLogger logger)
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
        
        var task = (Task<TResponse>)method.Invoke(handler,
            [request, cancellationToken]);

        return await task;
    }
}