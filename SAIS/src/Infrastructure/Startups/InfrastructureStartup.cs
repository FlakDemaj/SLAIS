using SAIS.Domain.Commom.Logger;
using SAIS.Infrastructure.DependencyInjection;
using SAIS.Infrastructure.InternalServices.Logging;

namespace SAIS.Infrastructure.Services;

public static class InfrastructureStartup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddInfrastructure();
    }
}