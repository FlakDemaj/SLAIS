namespace Presentation.Controllers;

public class LoginRequest
{
    public string LoginName { get; set; }
    
    public string Password { get; set; }
    
    public Guid DeviceGuid { get; set; }
    
    public string DeviceName { get; set; }

}