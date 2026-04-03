using Application.Common.DTOs;
using Application.Public.Users;
using Application.Public.Users.Querys.GetUser;
using Application.Public.Users.Querys.GetUsers;
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
    {
    }

    [HttpGet]
    [Authorize(Roles = $"{Role.Teacher},{Role.Admin},{Role.Server},{Role.SuperAdmin}")]
    public async Task<ActionResult<List<GetUsersResponseDto>>> GetUsersAsync(CancellationToken cancellationToken)
    {
        return await _mediator.SendAsync(
            new GetUsersQuery(),
            Authentication,
            cancellationToken);
    }

    [HttpGet("{publicId:int}")]
    [Authorize(Roles = $"{Role.Teacher},{Role.Admin},{Role.Server},{Role.SuperAdmin}")]
    public async Task<ActionResult<GetUserResponseDto>> GetUserByIdAsync(
        [FromRoute] int publicId,
        CancellationToken cancellationToken)
    {
        return await _mediator.SendAsync(
            new GetUserQuery { PublicId = publicId },
            Authentication,
            cancellationToken);
    }
}
