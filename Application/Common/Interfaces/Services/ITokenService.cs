using System.Net;
using Application.Authentication.Commands;
using SAIS.Domain.Users;

namespace Application.Common;

public interface ITokenService
{
    string GenerateAccessToken(UserEntity user);
    
    Task<(Guid, int)> GenerateRefreshToken(
        LoginCommand request,
        Guid userGuid);
    
    Task<bool> ValidateRefreshTokenAsync(string refreshToken);
}