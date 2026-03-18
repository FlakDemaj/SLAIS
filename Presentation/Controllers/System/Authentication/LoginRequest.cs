namespace Presentation.Controllers;

public class LoginRequest
{
    public required string LoginName { get; set; }

    public required string Password { get; set; }

    public required Guid DeviceGuid { get; set; }

    public required string DeviceName { get; set; }

}
