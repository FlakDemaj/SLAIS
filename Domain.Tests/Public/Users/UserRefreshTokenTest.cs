using System.Net;

using Domain.Systems.RefreshToken;

using FluentAssertions;

using Xunit;

namespace Domain.Tests.Public.Users;

public class UserRefreshTokenTest : UserTestBase
{
    private readonly RefreshTokenEntity _refreshTokenEntity;

    public UserRefreshTokenTest()
    {
        _refreshTokenEntity = _user.CreateRefreshToken(
            5,
            Guid.CreateVersion7(),
            "Iphone Galaxy S8 Ultra",
            IPAddress.Loopback);
    }

    [Fact]
    public void CreateRefreshTokenForUser_NotBeNull()
    {
        _user.RefreshTokens.Should().NotBeEmpty();
        _user.RefreshTokens.Should().ContainSingle();
        _user.RefreshTokens.First().Should().BeEquivalentTo(_refreshTokenEntity);
    }

    [Fact]
    public void CreateRefreshTokenForUser_ShouldSetCorrectUserGuid()
    {
        _refreshTokenEntity.UserGuid.Should().Be(_user.Guid);
    }

    [Fact]
    public void CreateRefreshTokenForUser_ShouldSetCorrectRefreshTokenGuid()
    {
        _refreshTokenEntity.RefreshToken.Should().NotBeEmpty();
    }

    [Fact]
    public void CreateRefreshTokenForUser_ShouldSetCorrectDeviceAttributes()
    {
        _refreshTokenEntity.DeviceName.Should().Be("Iphone Galaxy S8 Ultra");
        _refreshTokenEntity.DeviceGuid.Should().NotBeEmpty();
    }

    [Fact]
    public void CreateRefreshTokenForUser_ShouldSetCorrectUser()
    {
        _refreshTokenEntity.UserGuid.Should().Be(_user.Guid);
    }

    [Fact]
    public void CreateRefreshTokenForUser_ShouldSetCorrectDateTimes()
    {
        _refreshTokenEntity.CreatedDate.Should().BeAfter(DateTime.MinValue);
        _refreshTokenEntity.ExpirationDate.Should().BeAfter(DateTime.UtcNow);
        _refreshTokenEntity.LastUsedDate.Should().BeAfter(DateTime.MinValue);
        _refreshTokenEntity.RevokedDate.Should().BeNull();
    }

    [Fact]
    public void CreateRefreshTokenForUser_ShouldSetCorrectRevoke()
    {
        _refreshTokenEntity.Revoked.Should().BeFalse();
    }

    [Fact]
    public void CreateRefreshTokenForUser_ShouldSetCorrectIpAddress()
    {
        _refreshTokenEntity.IpAddress.Should().Be(IPAddress.Loopback);
    }

}
