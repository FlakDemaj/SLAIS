using Infrastructure.Persistence.Context;

using Microsoft.EntityFrameworkCore;

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

    private async Task CleanDatabaseAsync()
    {
        var qualifiedTableNames = _dbContext.Model
            .GetEntityTypes()
            .Select(e => new
            {
                Schema = e.GetSchema() ?? "public",
                Table = e.GetTableName()
            })
            .Where(e => e.Table is not null)
            .Where(e => !_excludedSchemas.Contains(e.Schema))
            .Select(e => $"\"{e.Schema}\".\"{e.Table}\"")
            .Distinct();

        var tables = string.Join(", ", qualifiedTableNames);

        await _dbContext.Database.ExecuteSqlRawAsync(
            $"TRUNCATE TABLE {tables} RESTART IDENTITY CASCADE");
    }
}
