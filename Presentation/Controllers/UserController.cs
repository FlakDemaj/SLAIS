using Application.Users.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class UserController : BaseRestController
{
    public UserController(IMediator mediator) :
        base(mediator)
    {
    }

    [HttpGet("{userGuid:Guid}")]
    public async Task<ActionResult<UserDto>> GetUserAsync(Guid userGuid)
    {
        var user = await _mediator.Send(new GetUserQuery(userGuid));
        return Ok(user);
    }
}