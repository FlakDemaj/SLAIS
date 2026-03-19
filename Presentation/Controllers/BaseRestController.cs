using Application.Utils.Interfaces.Mediator;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Authorize]
[ApiController]
[Route("rest/[controller]")]
public class BaseRestController : ControllerBase
{
    protected readonly IMediator _mediator;

    public BaseRestController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
