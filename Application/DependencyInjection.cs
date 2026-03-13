using System.Reflection;
using Application.Utils;
using Application.Utils.Logger;
using Application.Utils.MediatR;
using Application.Utils.MediatR.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SAIS.Infrastructure.InternalServices.Logging;

namespace Application.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApplicationLayer(this IServiceCollection service)
    {
        service.AddSingleton(typeof(ISAISLogger<>), typeof(SAISLogger<>));
    }

    public static void AddMediator(this IServiceCollection services)
    {
        var handlerInterface = typeof(IRequestHandler<,>);

        var handlers = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces()
                    .Where(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == handlerInterface),
                (type, iface) => new { type, iface });

        foreach (var handler in handlers)
        {
            services.AddTransient(handler.iface, handler.type);
        }

        services.AddScoped<IMediatR, MediatR>();
        AddPipeline(services);
    }

    private static void AddPipeline(IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineTransactionBehavior<,>), typeof(PipelineTransactionBehavior<,>));
    }
}