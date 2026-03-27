using Application.Common;
using Application.Common.Interfaces;
using Application.Utils.Interfaces.Mediator;

using Domain.Common;
using Domain.Common.Exceptions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Authorize]
[ApiController]
[Route("rest/[controller]")]
public class BaseRestController : ControllerBase
{
    protected readonly IMediator _mediator;

    protected IAuthentication Authentication
    {
        get
        {
            HttpContext.Items.TryGetValue(nameof(IAuthentication), out var value);
            if (value is IAuthentication userAuthentication)
            {
                return userAuthentication;
            }
#pragma warning disable S2344
            throw new SlaisException(CommonErrorCodes.DefaultErrorCode);
#pragma warning restore S2344            
        }
    }

    public BaseRestController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
