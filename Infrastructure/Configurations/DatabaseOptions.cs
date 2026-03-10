using System.Text.Json.Serialization;

namespace Infrastructure.Configurations;

public class DatabaseOptions
{
    [JsonPropertyName("Connection_String")]
    public string ConnectionString { get; set; } = "";
}