using EvolveDb;

using Infrastructure.Configurations;
using Infrastructure.Persistence.Context;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Npgsql;

using Testcontainers.PostgreSql;

using Xunit;

namespace Infrastructure.Tests;

public class PostgreSqlContainerFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer;

    public SlaisDbContext SlaisDbContext { get; private set; } = null!;

    public PostgreSqlContainerFixture()
    {
        _postgresContainer = new PostgreSqlBuilder("postgres:15-alpine")
            .WithDatabase("testdb")
            .WithUsername("testuser")
            .WithPassword("testpassword")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();

        var connectionString = _postgresContainer.GetConnectionString();

        var databaseOptions = Options.Create(new DatabaseOptions
        {
            ConnectionString = connectionString
        });

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });

        SlaisDbContext = new SlaisDbContext(databaseOptions, loggerFactory);

        RunEvolveMigrations(connectionString);
    }

    private static void RunEvolveMigrations(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString)
        {
            CommandTimeout = 3600
        };

        using var connection = new NpgsqlConnection(builder.ConnectionString);

        var evolve = new Evolve(connection)
        {
            Locations = [CreateMigrationsFolderPath()],
            Schemas = ["evolve"],
            IsEraseDisabled = false,
            OutOfOrder = true
        };

        evolve.Migrate();
    }

    private static string CreateMigrationsFolderPath()
    {
        var baseDirectory = AppContext.BaseDirectory;
        return Path.Combine(baseDirectory, "Persistence", "Migrations");
    }

    public async Task DisposeAsync()
    {
        await SlaisDbContext.DisposeAsync();
        await _postgresContainer.DisposeAsync();
    }
}
