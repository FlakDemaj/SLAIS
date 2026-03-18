using Application.Common.Options;
using Application.Utils.Logger;

using AutoMapper;

using Microsoft.Extensions.Options;

namespace Application.Common.Base;

public abstract class BaseHandler<T> where T : class
{
    protected readonly ISlaisLogger<T> Logger;

    protected readonly IMapper Mapper;

    protected readonly CommonOptions CommonOptions;

    protected BaseHandler(
        ISlaisLogger<T> logger,
        IMapper mapper,
        IOptions<CommonOptions> commonOptions)
    {
        Logger = logger;
        Mapper = mapper;
        CommonOptions = commonOptions.Value;
    }
}
