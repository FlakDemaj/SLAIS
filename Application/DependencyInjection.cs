using Infrastructure.AutoMappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SAIS.Domain.Users;

namespace Application;

public static class DependencyInjection
{

    public static void AddApplicationLayer(this IServiceCollection services)
    {
        AutoAutoMapper(services);
        
        services.AddScoped<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();
    }

    private static void AutoAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AuthMappingProfile).Assembly);
    }
}