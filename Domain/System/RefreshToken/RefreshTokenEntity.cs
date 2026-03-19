using System.Net;

namespace Domain.Systems.RefreshToken;

public class RefreshTokenEntity : RefreshTokenNavigationPropertyEntity
{
    public Guid RefreshToken { get; private set; }

    public DateTime ExpirationDate { get; init; }

    public Guid DeviceGuid { get; private set; }

    public string DeviceName { get; private set; }

    public IPAddress IpAddress { get; private set; }

    public bool Revoked { get; private set; }

    public DateTime CreatedDate { get; init; }

    public DateTime LastUsedDate { get; private set; }

    public DateTime? RevokedDate { get; private set; }

    public Guid UserGuid { get; private set; }

    private RefreshTokenEntity(
        int expirationInDate,
        Guid deviceGuid,
        string deviceName,
        IPAddress ipAddress,
        Guid userGuid)
    {
        RefreshToken = Guid.CreateVersion7();
        ExpirationDate = DateTime.UtcNow.AddDays(expirationInDate);
        DeviceGuid = deviceGuid;
        DeviceName = deviceName;
        IpAddress = ipAddress;
        Revoked = false;
        CreatedDate = DateTime.UtcNow;
        LastUsedDate = DateTime.UtcNow;
        RevokedDate = null;
        UserGuid = userGuid;
    }

    internal static RefreshTokenEntity CreateRefreshToken(
        int expirationInDate,
        Guid deviceGuid,
        string deviceName,
        IPAddress ipAddress,
        Guid userGuid)
    {
        return new RefreshTokenEntity(
            expirationInDate,
            deviceGuid,
            deviceName,
            ipAddress,
            userGuid);
    }

    public int GetExpirationInDays()
    {
        return ExpirationDate.Subtract(LastUsedDate).Days;
    }
}
