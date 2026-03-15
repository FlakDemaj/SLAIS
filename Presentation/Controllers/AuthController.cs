using Application.Authentication.Commands;
using Application.Authentication.DTOs;
using Application.Utils.MediatR.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class AuthController : BaseRestController
{
    
    public AuthController(IMediatR mediator)
        : base(mediator)
    {}

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDTO>> login([FromBody] LoginCommand loginCommand)
    {
        return await _mediator.SendAsync(loginCommand);
    }
}