using Application.Authentication.Commands.Login;
using Application.Utils.Interfaces.Mediator;
using Application.Utils.Interfaces.Transaction;

namespace Application.Authentication.Commands.ValidateRefreshToken;

public class ValidateRefreshTokenCommand : IRequest<AccessTokenResponseDto>, INoTransaction
{
    public required Guid RefreshToken { get; init; }
}
