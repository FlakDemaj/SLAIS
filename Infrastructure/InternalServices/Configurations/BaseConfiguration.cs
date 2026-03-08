using Microsoft.Extensions.Configuration;

namespace SAIS.Infrastructure.InternalServices.Configurations;

public abstract class BaseConfiguration
{
    protected static IConfiguration Configuration { get; set; }
}