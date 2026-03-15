using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SAIS.Domain.Users;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Infrastructure.InternalServices;

public class TokenService : ITokenService
{
    private readonly TokenOptions _tokenOptions;

    public TokenService(IOptions<TokenOptions> tokenOptions)
    {
        _tokenOptions = tokenOptions.Value;
    }
    
    public string GenerateAccessToken(UserEntity user)
    {
        var claims = new List<Claim>
        {

            new(JwtRegisteredClaimNames.Sub, user.Guid.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.CreateVersion7().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
          
            new(ClaimTypes.Role, user.Role.ToString()),
            new("InstituteGuid", user.Institute.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_tokenOptions.Key));
        
        var creds = new SigningCredentials(key,
            SecurityAlgorithms.HmacSha512);


        var token = new JwtSecurityToken
        (
            issuer: _tokenOptions.Issuer, 
            audience: _tokenOptions.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_tokenOptions.ExpiresInMinutes),
            signingCredentials: creds
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public Task<string> GenerateRefreshToken()
    {
        throw new NotImplementedException();
    }

    public Task<bool> ValidateRefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }
}