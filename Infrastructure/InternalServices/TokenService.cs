using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Application.Authentication.Commands;
using Application.Authentication.Commands.Login;
using Application.Common;
using Application.Interfaces;

using AutoMapper;

using Domain.Systems.RefreshToken;

using Infrastructure.Configurations;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using SLAIS.Domain.Users;

using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Infrastructure.InternalServices;

public class TokenService : ITokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    private readonly TokenOptions _tokenOptions;

    private readonly IMapper _mapper;

    private readonly JwtSecurityTokenHandler _tokenHandler;

    public TokenService(
        IRefreshTokenRepository refreshTokenRepository,
        IOptions<TokenOptions> tokenOptions,
        IMapper mapper)
    {
        _tokenOptions = tokenOptions.Value;
        _mapper = mapper;
        _tokenHandler = new JwtSecurityTokenHandler();
        _refreshTokenRepository = refreshTokenRepository;
    }

    public string GenerateAccessToken(UserEntity user)
    {
        List<Claim> claims = CreateUserClaims(user);

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_tokenOptions.Key));

        var creds = new SigningCredentials(key,
            SecurityAlgorithms.HmacSha512);

        JwtSecurityToken token = CreateToken(
            claims, creds);

        return _tokenHandler.WriteToken(token);
    }

    public async Task<GeneratedRefreshTokenResult> GenerateRefreshToken(LoginCommand request, Guid userGuid)
    {
        RefreshTokenEntity refreshToken = _mapper.Map<RefreshTokenEntity>(
            (request, userGuid, _tokenOptions.ExpiresInDays));

        await _refreshTokenRepository.CreateAsync(refreshToken);

        return new GeneratedRefreshTokenResult
        {
            RefreshToken = refreshToken.RefreshToken,
            ExpiresIn = _tokenOptions.ExpiresInDays
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
            issuer: _tokenOptions.Issuer,
            audience: _tokenOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_tokenOptions.ExpiresInMinutes),
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
