using FluentAssertions;

using Infrastructure.Configurations;
using Infrastructure.InternalServices;

using Microsoft.Extensions.Options;

using SLAIS.Domain.Users;

using Xunit;

namespace Infrastructure.Tests;

public class TokenServiceTests
{
    private readonly TokenService _tokenService;

    public TokenServiceTests()
    {
        var accessTokenOptions = Options.Create(new AccessTokenOptions
        {
            Audience = "TestAudience",
            ExpiresInMinutes = 30,
            Issuer = "TestIssuer",
            Key = "SLAIS-Testing-Secret-Key-For-HmacSha512-Must-Be-64-Characters-!!"
        });

        _tokenService = new TokenService(accessTokenOptions);
    }

    [Fact]
    public void GenerateAccessToken_CreateToken()
    {
        var user = UserEntity.CreateAdmin(
            Guid.CreateVersion7(),
            "test@slais.de",
            "HashedPassword",
            "testAdmin",
            "Max",
            "Mustermann",
            Guid.CreateVersion7());

        var token = _tokenService.GenerateAccessToken(user);

        token.Should().NotBeNullOrEmpty();
    }
}
