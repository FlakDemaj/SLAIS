using Application.Utils.Logger;

namespace Application.Common;

public abstract class BaseHandler<T> where T : class
{
    protected ISAISLogger<T> _logger;

    protected BaseHandler(ISAISLogger<T> logger)
    {
        _logger = logger;
    }
}