using Microsoft.Extensions.Configuration;

namespace Infrastructure.Configurations;

public class TokenOptions
{
    public string Issuer { get; set; }

    public string Audience { get; set; }

    public string Key { get; set; }
    
    [ConfigurationKeyName("Expires_In_Minutes")]
    public int ExpiresInMinutes { get; set; }
}