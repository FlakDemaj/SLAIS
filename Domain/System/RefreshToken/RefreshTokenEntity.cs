using System.Net;

namespace Domain.Systems.RefreshToken;

public class RefreshTokenEntity : RefreshTokenNavigationPropertyEntity
{
    public Guid RefreshToken { get; set; }

    public DateTime ExpirationDate { get; set; }

    public Guid DeviceGuid { get; set; }

    public string DeviceName { get; set; }

    public IPAddress IpAddress { get; set; }

    public bool Revoked { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastUsedDate { get; set; }

    public DateTime? RevokedDate { get; set; }

    public Guid UserGuid { get; set; }
}
