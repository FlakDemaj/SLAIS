using Application.Utils.Logger;

using AutoMapper;

namespace Application.Common;

public abstract class BaseHandler<T> where T : class
{
    protected ISlaisLogger<T> Logger;

    protected IMapper Mapper;

    protected BaseHandler(
        ISlaisLogger<T> logger,
        IMapper mapper)
    {
        Logger = logger;
        Mapper = mapper;
    }
}
