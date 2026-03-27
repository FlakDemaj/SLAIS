using Application.Authentication.Commands.Login;
using Application.Utils.Interfaces.Mediator;

namespace Application.Authentication.Commands.ValidateRefreshToken;

public class ValidateRefreshTokenCommand : IRequest<AccessTokenResponseDto>
{
    public required Guid RefreshToken { get; init; }
}
