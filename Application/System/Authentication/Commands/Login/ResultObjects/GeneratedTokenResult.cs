namespace Application.Authentication.Commands.Login;

public sealed class GeneratedTokenResult
{
    public required string AccessToken { get; init; }

    public required Guid RefreshToken { get; init; }

    public required int ExpiresIn { get; init; }
}
