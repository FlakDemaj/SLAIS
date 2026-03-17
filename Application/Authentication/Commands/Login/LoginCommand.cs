using System.Net;

using Application.Authentication.Commands.Login;
using Application.Utils.Interfaces.MediatR;

namespace Application.Authentication.Commands;

public class LoginCommand : IRequest<GeneratedTokenResult>
{
    public string LoginName { get; init; }

    public string Password { get; init; }

    public Guid DeviceGuid { get; init; }

    public string DeviceName { get; init; }

    public IPAddress IpAddress { get; set; }
}
