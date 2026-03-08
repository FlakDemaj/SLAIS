using SAIS.Domain.Commom.Logger;
using SAIS.Infrastructure.InternalServices.Logging;

namespace SAIS.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ISAISLogger, SentryAppLogger>();
        return services;
    }
}