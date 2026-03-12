using Autofac;
using Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Z.EntityFramework.Plus;

namespace Infrastructure.Persistence.Context;

public class SAISDbContext : DbContext
{
    private string _connectionString { get; init;  }
    
    private ILoggerFactory _iLoggerFactory { get; init; }
    
    public SAISDbContext(
        IOptions<DatabaseOptions> databaseOptions,
        ILoggerFactory iLoggerFactory)
    {
        _connectionString = databaseOptions.Value.ConnectionString;
        _iLoggerFactory = iLoggerFactory;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(optionsBuilder.IsConfigured)
        {
            return;
        }

        optionsBuilder.UseNpgsql(
            _connectionString,
            o =>
            {
                o.CommandTimeout(120);
            });

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production")
        {
            optionsBuilder.UseLoggerFactory(_iLoggerFactory);
        }

        EntityFrameworkPlusManager.IsCommunity = true;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(SAISDbContext).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
    }
}
