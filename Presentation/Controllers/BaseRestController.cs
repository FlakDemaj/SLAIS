using Application.Utils.MediatR.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("rest/[controller]")]
public class BaseRestController : ControllerBase
{
    protected readonly IMediatR _mediator;

    public BaseRestController(IMediatR mediator)
    {
        _mediator = mediator;
    }
}