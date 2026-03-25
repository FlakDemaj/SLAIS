using Application.Common.Options;
using Application.Utils.Logger;

using AutoMapper;

using Microsoft.Extensions.Options;

namespace Application.Common.Base;

public abstract class BaseHandler<T> where T : class
{
    protected readonly ISlaisLogger<T> _logger;

    protected BaseHandler(
        ISlaisLogger<T> logger)
    {
        _logger = logger;
    }
}
