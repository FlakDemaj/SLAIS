using System.Net;

using ClassLibrary1.Builders;

using Domain.Public.Users;
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
        var act = () => _refreshTokenEntity = _user.CreateRefreshToken(
            0,
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

    #region Get

    [Fact]
    public void GetRefreshTokenExpireDate_ShouldReturnCorrectExpireDate()
    {
        _refreshTokenEntity.GetExpirationInDays().Should().Be(29);
    }

    #endregion

    #region Validate

    [Fact]
    public void ValidateRefreshToken_ShouldReturnTrue()
    {
        var refreshToken = _user.RefreshTokens.First().RefreshToken;
        var isValid = _user.ValidateRefreshToken(refreshToken);

        isValid.Should().BeTrue();
        _user.RefreshTokens.First().LastUsedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void ValidateRefreshToken_ShouldReturnThrowException_UserIsBlocked()
    {
        BlockUser();

        var refreshToken = _user.RefreshTokens.First().RefreshToken;

        var act = () => _user.ValidateRefreshToken(refreshToken);

        act.ThrowsException(UserErrorCodes.UserIsBlocked);
    }

    [Fact]
    public void ValidateRefreshToken_ShouldReturnFalse_TokenIsNotFound()
    {
        var token = _user.RefreshTokens.First();

        var isValid = _user.ValidateRefreshToken(token.Guid);

        isValid.Should().BeFalse();
    }

    [Fact]
    public void ValidateRefreshToken_ShouldReturnFalse_TokenIsRevoked()
    {
        var token = new RefreshTokenEntityBuilder().BuildRevoked(_user);

        var isValid = _user.ValidateRefreshToken(token.RefreshToken);

        isValid.Should().BeFalse();
    }

    [Fact]
    public void ValidateRefreshToken_ShouldReturnFalse_TokenIsExpired()
    {
        var token = new RefreshTokenEntityBuilder().BuildExpired(_user);

        var isValid = _user.ValidateRefreshToken(token.RefreshToken);

        isValid.Should().BeFalse();
    }

    #endregion
}
