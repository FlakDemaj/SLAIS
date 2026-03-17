namespace Infrastructure.Configurations;

public class AccessTokenOptions
{
    public string Issuer { get; init; }

    public string Audience { get; init; }

    public string Key { get; init; }

    public int ExpiresInMinutes { get; init; }

}
