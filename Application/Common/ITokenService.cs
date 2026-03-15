using SAIS.Domain.Users;

namespace Application.Common;

public interface ITokenService
{
    string GenerateAccessToken(UserEntity user);
    
    Task<string> GenerateRefreshToken();
    
    Task<bool> ValidateRefreshTokenAsync(string refreshToken);
}