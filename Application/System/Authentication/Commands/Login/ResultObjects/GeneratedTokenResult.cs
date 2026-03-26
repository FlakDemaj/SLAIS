namespace Application.Authentication.Commands.Login;

public sealed class GeneratedTokenResult
{
    public required GeneratedAccessTokenResult GeneratedAccessToken { get; init; }

    public required GeneratedRefreshTokenResult RefreshToken { get; init; }

}
