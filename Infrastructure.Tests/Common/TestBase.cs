using Infrastructure.Persistence.Context;
using Infrastructure.Repositorys;

using Microsoft.EntityFrameworkCore;

using SLAIS.Domain.Commom;

using Xunit;

namespace Infrastructure.Tests.Common;

[Collection(nameof(PostgresContainerCollection))]
public abstract class TestBase : IAsyncLifetime
{
    private readonly SlaisDbContext _dbContext;

    private static readonly HashSet<string> _excludedSchemas = ["evolve"];

    protected TestBase(PostgreSqlContainerFixture fixture)
    {
        _dbContext = fixture.SlaisDbContext;
    }

    public virtual async Task InitializeAsync()
    {
        await CleanDatabaseAsync();
    }

    public virtual Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    protected async Task<T?> GetCreatedEntityByGuid<T>(Guid userGuid)
        where T : BaseGuidEntity
    {
        return await _dbContext
            .GetTrackingSet<T>()
            .FirstOrDefaultAsync(rt => rt.Guid == userGuid);
    }

    private async Task CleanDatabaseAsync()
    {
        var qualifiedTableNames = _dbContext.Model
            .GetEntityTypes()
            .Select(e =>
            {
                return new
                {
                    Schema = e.GetSchema() ?? "public",
                    Table = e.GetTableName()
                };
            })
            .Where(e =>
            {
                return e.Table is not null;
            })
            .Where(e =>
            {
                return !_excludedSchemas.Contains(e.Schema);
            })
            .Select(e =>
            {
                return $"\"{SanitizeIdentifier(e.Schema)}\".\"{SanitizeIdentifier(e.Table!)}\"";
            })
            .Distinct();

        var tables = string.Join(", ", qualifiedTableNames);

#pragma warning disable EF1002
        await _dbContext.Database.ExecuteSqlRawAsync(
            $"TRUNCATE TABLE {tables} RESTART IDENTITY CASCADE");
#pragma warning restore EF1002
    }

    private static string SanitizeIdentifier(string identifier)
    {
        return identifier.Replace("\"", string.Empty);
    }
}
