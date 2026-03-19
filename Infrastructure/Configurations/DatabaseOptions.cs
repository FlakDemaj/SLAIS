using Microsoft.Extensions.Configuration;

namespace Infrastructure.Configurations;

public class DatabaseOptions
{
    [ConfigurationKeyName("Connection_String")]
    public required string ConnectionString { get; init; }
}
