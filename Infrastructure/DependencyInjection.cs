using System.Reflection;
using Application;
using Application.Common;
using Application.Interfaces;
using Application.Utils;
using Application.Utils.Logger;
using Application.Utils.MediatR;
using Application.Utils.MediatR.Interfaces;
using Infrastructure.InternalServices;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Context;
using Infrastructure.Repositorys;
using Infrastructure.Transaction;
using Microsoft.Extensions.DependencyInjection;
using SAIS.Infrastructure.InternalServices.Logging;

namespace SAIS.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        AddPipeline(services);
        AddDatabase(services);
        AddRepositories(services);
        AddInternalServices(services);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    }
    
    private static void AddPipeline(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddDatabase(IServiceCollection services)
    {
        services.AddScoped<MigrationManager>();
        services.AddDbContext<SAISDbContext>();
    }
    
    private static void AddInternalServices(IServiceCollection services)
    {
        AddMediator(services);
        services.AddSingleton(typeof(ISAISLogger<>), typeof(SAISLogger<>));
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
    }

    private static void AddMediator(IServiceCollection services)
    {
        var handlerInterface = typeof(IRequestHandler<,>);

        var handlers = Assembly.GetAssembly(typeof(ApplicationAssemblyMarker)).GetTypes()
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
        services.AddTransient(typeof(IPipelineTransactionBehavior<,>), typeof(PipelineTransactionBehavior<,>));
    }

}