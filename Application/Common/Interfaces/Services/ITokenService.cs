using Application.Authentication.Commands;
using Application.Authentication.Commands.Login;

using SLAIS.Domain.Users;

namespace Application.Common.Interfaces.Services;

public interface ITokenService
{
    string GenerateAccessToken(UserEntity user);

    Task<GeneratedRefreshTokenResult> GenerateRefreshToken(
        LoginCommand request,
        Guid userGuid);

    Task<bool> ValidateRefreshTokenAsync(string refreshToken);
}
