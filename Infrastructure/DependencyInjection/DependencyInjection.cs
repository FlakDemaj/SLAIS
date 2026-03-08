using Application.Utils.Logger;
using Microsoft.Extensions.DependencyInjection;
using SAIS.Infrastructure.InternalServices.Logging;

namespace SAIS.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ISAISLogger, SentryAppLogger>();
        return services;
    }
    
}