using Application.Utils.Logger;

using AutoMapper;

namespace Application.Common.Base;

public abstract class BaseHandler<T> where T : class
{
    protected readonly ISlaisLogger<T> _logger;

    protected readonly IMapper _mapper;

    protected BaseHandler(
        IMapper mapper,
        ISlaisLogger<T> logger)
    {
        _mapper = mapper;
        _logger = logger;
    }
}
