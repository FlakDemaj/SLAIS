using Microsoft.Extensions.Configuration;

namespace Infrastructure.Configurations;

public class DatabaseOptions
{
    [ConfigurationKeyName("Connection_String")]
    public string ConnectionString { get; set; } = "";
}
