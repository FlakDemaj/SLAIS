namespace Application.Authentication.Commands.Login;

public class GeneratedRefreshTokenResult
{
    public required Guid RefreshToken { get; init; }

    public required int RefreshTokenExpiresInDays { get; init; }
}
