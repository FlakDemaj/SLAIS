using System.Net;
using SAIS.Domain.Commom;

namespace Domain.Systems.RefreshToken;

public class RefreshTokenEntity : RefreshTokenNavigationPropertyEntity
{
    public Guid RefreshToken { get; set; }
    
    public DateTime ExpirationDate { get; set; }
    
    public Guid DeviceGuid { get; set; }
    
    public Guid DeviceName { get; set; }
    
    public IPAddress IPAddress { get; set; }
    
    public bool Revoked { get; set; }

    public DateTime CreatedDate { get; set; }
    
    public DateTime LastUsedDate { get; set; }
    
    public DateTime? RevokedDate { get; set; }
    
    public Guid UserGuid { get; set; }
}