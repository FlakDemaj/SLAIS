using Application.Utils;
using Application.Utils.Logger;
using Microsoft.Extensions.DependencyInjection;
using SAIS.Infrastructure.InternalServices.Logging;

namespace Application.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApplicationLayer(this IServiceCollection service)
    {
        service.AddScoped<ISAISLogger, SAISLogger>();
    }
}