using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

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