using SLAIS.Domain.Users;

namespace Application.Common.Interfaces.Services;

public interface ITokenService
{
    string GenerateAccessToken(UserEntity user);

    Task<bool> ValidateRefreshTokenAsync(string refreshToken);
}
