using System.Net;

using Application.Authentication.Commands.Login;
using Application.Utils.MediatR.Interfaces;

namespace Application.Authentication.Commands;

public class LoginCommand : IRequest<GeneratedTokenResult>
{
    public string LoginName { get; set; }

    public string Password { get; set; }

    public Guid DeviceGuid { get; set; }

    public string DeviceName { get; set; }

    public IPAddress IpAddress { get; set; }
}
