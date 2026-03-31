using Application.Public.Users;
using Application.Public.Users.Commands.GetUsers;
using Application.Utils.Interfaces.Mediator;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Presentation.Utils;

namespace Presentation.Controllers.Public.Users;

public class UserController : BaseRestController
{
    public UserController(
    IMediator mediator)
        : base(mediator)
    {}

    [HttpGet]
    [Authorize(Roles = $"{Role.Teacher},{Role.Admin},{Role.Server},{Role.SuperAdmin}")]
    public async Task<ActionResult<List<GetUserResponseDto>>> GetUsersAsync(CancellationToken cancellationToken)
    {
        return await _mediator.SendAsync(
            new GetUsersCommand(),
            cancellationToken,
            Authentication);
    }
}
