using Application.Utils.Interfaces.MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Authorize]
[ApiController]
[Route("rest/[controller]")]
public class BaseRestController : ControllerBase
{
    protected readonly IMediatR Mediator;

    public BaseRestController(IMediatR mediator)
    {
        Mediator = mediator;
    }
}
