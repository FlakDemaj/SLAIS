using Application.Authentication.Commands;
using Application.Authentication.Commands.Login;

using SAIS.Domain.Users;

namespace Application.Common;

public interface ITokenService
{
    string GenerateAccessToken(UserEntity user);

    Task<GeneratedRefreshTokenResult> GenerateRefreshToken(
        LoginCommand request,
        Guid userGuid);

    Task<bool> ValidateRefreshTokenAsync(string refreshToken);
}
