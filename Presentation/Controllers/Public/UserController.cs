using Application.Users.DTOs;
using Application.Utils.Interfaces.MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class UserController : BaseRestController
{
    public UserController(IMediatR mediator) :
        base(mediator)
    {
    }

    [HttpGet("{userGuid:Guid}")]
    public async Task<ActionResult<GetUserDto>> GetUserAsync(Guid userGuid)
    {
        var user = await Mediator.SendAsync(new GetUserQuery(userGuid));
        return Ok(user);
    }
}
