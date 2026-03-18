using Application.Authentication.Commands;

using Domain.Systems.RefreshToken;

using SLAIS.Domain.Users;

namespace Application.Common.Interfaces.Services;

public interface ITokenService
{
    string GenerateAccessToken(UserEntity user);

    Task<RefreshTokenEntity> GenerateRefreshToken(
        LoginCommand request,
        Guid userGuid);

    Task<bool> ValidateRefreshTokenAsync(string refreshToken);
}
