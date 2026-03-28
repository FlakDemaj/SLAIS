using System.Net;

using Application.Authentication.Commands.Login;
using Application.Utils.Interfaces.Mediator;
using Application.Utils.Interfaces.Transaction;

namespace Application.Authentication.Commands;

public class LoginCommand : IRequest<GeneratedTokenResult>, INoTransaction
{
    public required string LoginName { get; init; }

    public required string Password { get; init; }

    public required Guid DeviceGuid { get; init; }

    public required string DeviceName { get; init; }

    public required IPAddress IpAddress { get; init; }
}
