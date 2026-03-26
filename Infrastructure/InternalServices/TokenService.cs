using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Application.Authentication.Commands.Login;
using Application.Common.Interfaces.Services;

using Infrastructure.Configurations;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using SLAIS.Domain.Users;

using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Infrastructure.InternalServices;

public class TokenService : ITokenService
{
    private readonly AccessTokenOptions _accessTokenOptions;

    private readonly JwtSecurityTokenHandler _tokenHandler;

    public TokenService(
        IOptions<AccessTokenOptions> tokenOptions)
    {
        _accessTokenOptions = tokenOptions.Value;
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public GeneratedAccessTokenResult GenerateAccessToken(UserEntity user)
    {
        var claims = CreateUserClaims(user);

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_accessTokenOptions.Key));

        var creds = new SigningCredentials(key,
            SecurityAlgorithms.HmacSha512);

        var token = CreateToken(
            claims, creds);

        var accessToken = _tokenHandler.WriteToken(token);

        return new GeneratedAccessTokenResult
        {
            AccessToken = accessToken,
            AccessTokenExpiresInMinutes = _accessTokenOptions.ExpiresInMinutes
        };
    }

    public Task<bool> ValidateRefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }

    private JwtSecurityToken CreateToken(
        List<Claim> claims,
        SigningCredentials creds)
    {
        var token = new JwtSecurityToken
        (
            issuer: _accessTokenOptions.Issuer,
            audience: _accessTokenOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_accessTokenOptions.ExpiresInMinutes),
            signingCredentials: creds
        );

        return token;
    }

    private static List<Claim> CreateUserClaims(UserEntity user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Guid.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.CreateVersion7().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),

            new(ClaimTypes.Role, user.Role.ToString()),
            new("InstituteGuid", user.InstituteUuid.ToString())
        };

        return claims;
    }
}
