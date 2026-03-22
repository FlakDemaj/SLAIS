using Xunit;

namespace Infrastructure.Tests.Common;

[CollectionDefinition(nameof(PostgresContainerCollection))]
public class PostgresContainerCollection : ICollectionFixture<PostgreSqlContainerFixture>;
