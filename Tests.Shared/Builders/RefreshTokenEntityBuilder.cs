using System.Net;

using Domain.System.RefreshToken;

using SLAIS.Domain.Users;

namespace ClassLibrary1.Builders;

public class RefreshTokenEntityBuilder
{
    private Guid _deviceGuid = Guid.NewGuid();
    private string _deviceName = "Test Device";
    private IPAddress _ipAddress = IPAddress.Loopback;

    public RefreshTokenEntityBuilder WithDeviceGuid(Guid deviceGuid)
    {
        _deviceGuid = deviceGuid;
        return this;
    }

    public RefreshTokenEntityBuilder WithDeviceName(string deviceName)
    {
        _deviceName = deviceName;
        return this;
    }

    public RefreshTokenEntityBuilder WithIpAddress(IPAddress ipAddress)
    {
        _ipAddress = ipAddress;
        return this;
    }

    private UserEntity Build(UserEntity user)
    {
        user.CreateRefreshToken(
            7,
            _deviceGuid,
            _deviceName,
            _ipAddress);

        return user;
    }

    public RefreshTokenEntity BuildExpired(UserEntity user)
    {
        var token = Build(user).RefreshTokens.Single();

        typeof(RefreshTokenEntity)
            .GetProperty(nameof(RefreshTokenEntity.ExpirationDate))!
            .SetValue(token, DateTime.UtcNow.AddDays(-1));

        return token;
    }

    public RefreshTokenEntity BuildRevoked(UserEntity user)
    {
        var token = Build(user).RefreshTokens.Single();

        typeof(RefreshTokenEntity)
            .GetProperty(nameof(RefreshTokenEntity.Revoked))!
            .SetValue(token, true);

        return token;
    }

    public UserEntity BuildWithOwnRefreshTokenGuid(UserEntity user, Guid guid)
    {
        var createdUser = Build(user);

        typeof(RefreshTokenEntity)
            .GetProperty(nameof(RefreshTokenEntity.RefreshToken))!
            .SetValue(createdUser.RefreshTokens.First(), guid);

        return createdUser;
    }

    public UserEntity BuildWithOwnRevokedGuidReturnsUserEntity(UserEntity user)
    {
        var createdUser = Build(user);

        typeof(RefreshTokenEntity)
            .GetProperty(nameof(RefreshTokenEntity.Revoked))!
            .SetValue(createdUser.RefreshTokens.First(), true);

        return createdUser;
    }
}
