using Application.Common;
using Application.Utils;
using Application.Utils.Logger;

using EvolveDb;

using Infrastructure.Configurations;

using Microsoft.Extensions.Options;

using Npgsql;

namespace Infrastructure.Persistence;

public class MigrationManager
{
    private const int CommandTimeoutInSeconds = 3600;

    private static string Location
    {
        get
        {
            return CreateMigrationsFolderPath();
        }
    }

    private const string Schema = "evolve";

    private readonly DatabaseOptions _options;
    private readonly ISlaisLogger<MigrationManager> _logger;

    public MigrationManager(IOptions<DatabaseOptions> options, ISlaisLogger<MigrationManager> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public void Migrate()
    {
        var builder = new NpgsqlConnectionStringBuilder(_options.ConnectionString)
        {
            CommandTimeout = CommandTimeoutInSeconds
        };

        using var connection = new NpgsqlConnection(builder.ConnectionString);

        var evolve = new Evolve(connection)
        {
            Locations = [Location],
            Schemas = [Schema],
            IsEraseDisabled = true,
            OutOfOrder = true
        };

        try
        {
            _logger.LogInformation("Starting database migration.");
            evolve.Migrate();
            _logger.LogInformation("Database migration completed successfully.");
        }
        catch (Exception exception)
        {
            _logger.LogError("Database migration failed.", exception);
            throw new SlaisException(CommonErrorCodes.DefaultErrorCode, exception);
        }
    }

    private static string CreateMigrationsFolderPath()
    {
        var baseDirectory = AppContext.BaseDirectory;
        return Path.Combine(baseDirectory, "Persistence", "Migrations");
    }

}
