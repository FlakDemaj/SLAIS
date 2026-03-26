using Application.Utils.Logger;

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
