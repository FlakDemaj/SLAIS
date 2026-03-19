namespace Infrastructure.Configurations;

public class AccessTokenOptions
{
    public required string Issuer { get; init; }

    public required string Audience { get; init; }

    public required string Key { get; init; }

    public required int ExpiresInMinutes { get; init; }

}
