using System.Net;

using Domain.System.RefreshToken;
using Domain.Systems.RefreshToken;
using Domain.Tests.Utils.Extensions;

using FluentAssertions;

using Xunit;

namespace Domain.Tests.Public.Users;

public class UserRefreshTokenTest : UserTestBase
{
    private RefreshTokenEntity _refreshTokenEntity;

    public UserRefreshTokenTest()
    {
        _refreshTokenEntity = _user.CreateRefreshToken(
            30,
            Guid.CreateVersion7(),
            "Iphone Galaxy S8 Ultra",
            IPAddress.Loopback);
    }

    #region Create

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

    [Fact]
    public void CreateRefreshToken_ShouldThrowExtension_ExpireDaysIsZero()
    {
        var expiresInDays = 0;

        var act = () => _refreshTokenEntity = _user.CreateRefreshToken(
            expiresInDays,
            Guid.CreateVersion7(),
            "Iphone Galaxy S8 Ultra",
            IPAddress.Loopback);

        act.ThrowsException(
            RefreshTokenErrorCodes.InvalidExpiration);
    }

    [Fact]
    public void CreateRefreshToken_ShouldThrowExtension_DeviceGuidIsEmpty()
    {
        var act = () => _refreshTokenEntity = _user.CreateRefreshToken(
            30,
            Guid.Empty,
            "Iphone Galaxy S8 Ultra",
            IPAddress.Loopback);

        act.ThrowsException(
            RefreshTokenErrorCodes.InvalidDeviceGuid);
    }

    [Fact]
    public void CreateRefreshToken_ShouldThrowExtension_DeviceNameIsEmpty()
    {
        var act = () => _refreshTokenEntity = _user.CreateRefreshToken(
            30,
            Guid.CreateVersion7(),
            " ",
            IPAddress.Loopback);

        act.ThrowsException(
            RefreshTokenErrorCodes.InvalidDeviceName);
    }

    #endregion

    [Fact]
    public void GetRefreshTokenExpireDate_ShouldReturnCorrectExpireDate()
    {
        _refreshTokenEntity.GetExpirationInDays().Should().Be(29);
    }

}
