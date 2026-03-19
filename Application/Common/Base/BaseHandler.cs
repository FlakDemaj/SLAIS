using Application.Common.Options;
using Application.Utils.Logger;

using AutoMapper;

using Microsoft.Extensions.Options;

namespace Application.Common.Base;

public abstract class BaseHandler<T> where T : class
{
    protected readonly ISlaisLogger<T> _logger;

    protected readonly IMapper _mapper;

    protected readonly CommonOptions _commonOptions;

    protected BaseHandler(
        ISlaisLogger<T> logger,
        IMapper mapper,
        IOptions<CommonOptions> commonOptions)
    {
        _logger = logger;
        _mapper = mapper;
        _commonOptions = commonOptions.Value;
    }
}
