using Application.Authentication.Commands.Login;

using SLAIS.Domain.Users;

namespace Application.Common.Interfaces.Services;

public interface ITokenService
{
    GeneratedAccessTokenResult GenerateAccessToken(UserEntity user);

    Task<bool> ValidateRefreshTokenAsync(string refreshToken);
}
