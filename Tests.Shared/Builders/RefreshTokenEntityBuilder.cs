using System.Net;

using Domain.System.RefreshToken;

using SLAIS.Domain.Users;

namespace Tests.Shared.Builders;

public class RefreshTokenEntityBuilder
{
    private Guid _deviceGuid = Guid.NewGuid();
    private string _deviceName = "Test Device";
    private readonly IPAddress _ipAddress = IPAddress.Loopback;
    private DateTime _expirationDate = DateTime.UtcNow.AddDays(7);
    private bool _revoked;
    private Guid _refreshTokenGuid = Guid.CreateVersion7();
    private readonly DateTime _createdDate = DateTime.UtcNow;
    private readonly DateTime _lastUsedDate = DateTime.UtcNow;
    private DateTime? _revokedDate;
    private Guid _userGuid = Guid.NewGuid();

    #region With Methods

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

    public RefreshTokenEntityBuilder WithExpiresInDays(int expiresInDays)
    {
        _expirationDate = DateTime.UtcNow.AddDays(expiresInDays);
        return this;
    }

    public RefreshTokenEntityBuilder WithExpired()
    {
        _expirationDate = DateTime.UtcNow.AddDays(-1);
        return this;
    }

    public RefreshTokenEntityBuilder WithRevoked()
    {
        _revoked = true;
        _revokedDate = DateTime.UtcNow;
        return this;
    }

    public RefreshTokenEntityBuilder WithRefreshTokenGuid(Guid guid)
    {
        _refreshTokenGuid = guid;
        return this;
    }

    private void WithUserGuid(Guid userGuid)
    {
        _userGuid = userGuid;
    }

    #endregion

    #region Build Methods

    private RefreshTokenEntity Build()
    {
        var token = CreateEmptyInstance();

        SetProperty(token, nameof(RefreshTokenEntity.RefreshToken), _refreshTokenGuid);
        SetProperty(token, nameof(RefreshTokenEntity.ExpirationDate), _expirationDate);
        SetProperty(token, nameof(RefreshTokenEntity.DeviceGuid), _deviceGuid);
        SetProperty(token, nameof(RefreshTokenEntity.DeviceName), _deviceName);
        SetProperty(token, nameof(RefreshTokenEntity.IpAddress), _ipAddress);
        SetProperty(token, nameof(RefreshTokenEntity.Revoked), _revoked);
        SetProperty(token, nameof(RefreshTokenEntity.CreatedDate), _createdDate);
        SetProperty(token, nameof(RefreshTokenEntity.LastUsedDate), _lastUsedDate);
        SetProperty(token, nameof(RefreshTokenEntity.RevokedDate), _revokedDate);
        SetProperty(token, nameof(RefreshTokenEntity.UserGuid), _userGuid);

        return token;
    }

    public RefreshTokenEntity Build(UserEntity user)
    {
        WithUserGuid(user.Guid);

        var token = Build();

        var refreshTokens = typeof(UserEntity)
            .GetProperty(nameof(UserEntity.RefreshTokens))!
            .GetValue(user) as ICollection<RefreshTokenEntity>;

        refreshTokens?.Add(token);

        return token;
    }

    #endregion

    #region Private

    private static RefreshTokenEntity CreateEmptyInstance()
    {
        return (RefreshTokenEntity)System.Runtime.CompilerServices
            .RuntimeHelpers
            .GetUninitializedObject(typeof(RefreshTokenEntity));
    }

    private static void SetProperty<TValue>(
        RefreshTokenEntity entity,
        string propertyName,
        TValue value)
    {
        typeof(RefreshTokenEntity)
            .GetProperty(propertyName)!
            .SetValue(entity, value);
    }

    #endregion
}
