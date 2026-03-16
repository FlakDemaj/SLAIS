using System.Net;
using Application.Authentication.Commands;
using Application.Utils.MediatR.Interfaces;
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
    public async Task<IActionResult> login([FromBody] LoginCommand loginCommand)
    {
        loginCommand.IPAddress = HttpContext.Connection.RemoteIpAddress ?? IPAddress.Loopback; 
        var tokens = await _mediator.SendAsync(loginCommand);

        HttpContext.Response.Cookies.Append(
            "RefreshToken",
            tokens.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(tokens.RefreshTokenExpirationDays)
            });
        
        return Ok(
            new
            {
                AccesToken =  tokens.AccessToken
            });
    }
}