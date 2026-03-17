using System.Reflection;

using Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Z.EntityFramework.Plus;

namespace Infrastructure.Persistence.Context;

public class SlaisDbContext : DbContext
{
    private string ConnectionString { get; init; }

    private ILoggerFactory LoggerFactory { get; init; }

    public SlaisDbContext(
        IOptions<DatabaseOptions> databaseOptions,
        ILoggerFactory iLoggerFactory)
    {
        ConnectionString = databaseOptions.Value.ConnectionString;
        LoggerFactory = iLoggerFactory;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        optionsBuilder.UseNpgsql(
            ConnectionString,
            o =>
            {
                o.CommandTimeout(120);
            });

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production")
        {
            optionsBuilder.UseLoggerFactory(LoggerFactory);
        }

        EntityFrameworkPlusManager.IsCommunity = true;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Assembly assembly = typeof(SlaisDbContext).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
    }
}
