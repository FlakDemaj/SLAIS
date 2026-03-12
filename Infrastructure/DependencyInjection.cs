using Application.Interfaces;
using Application.Utils.Logger;
using Infrastructure.Persistence.Context;
using Infrastructure.Repositorys;
using Microsoft.Extensions.DependencyInjection;
using SAIS.Infrastructure.InternalServices.Logging;

namespace SAIS.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<SAISDbContext>();
        services.AddSingleton<ISAISLogger, SAISLogger>();
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }
    
}