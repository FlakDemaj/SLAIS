using System.Net;

using Application.Authentication.Commands;
using Application.Authentication.Commands.Login;
using Application.Utils.Interfaces.MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class AuthController : BaseRestController
{
    public AuthController(
        IMediatR mediator)
        : base(mediator)
    {
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequest loginRequest)
    {
        var loginCommand = MapLoginRequest(loginRequest, HttpContext);
        var tokens = await Mediator.SendAsync(loginCommand);

        HttpContext.Response.Cookies.Append(
            "RefreshToken",
            tokens.RefreshToken.ToString(),
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(tokens.ExpiresIn)
            });

        return Ok(
            new LoginResponseDto
            {
                AccessToken = tokens.AccessToken
            }
        );
    }

    private static LoginCommand MapLoginRequest(
        LoginRequest loginRequest,
        HttpContext context)
    {
        return new LoginCommand
        {
            LoginName = loginRequest.LoginName,
            Password = loginRequest.Password,
            IpAddress = context.Connection.RemoteIpAddress ?? IPAddress.Loopback,
            DeviceName = loginRequest.DeviceName,
            DeviceGuid = loginRequest.DeviceGuid
        };
    }
}
