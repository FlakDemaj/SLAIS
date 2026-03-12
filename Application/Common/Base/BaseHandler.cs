using Application.Utils.Logger;

namespace Application.Common;

public abstract class BaseHandler
{
    protected ISAISLogger _logger;

    protected BaseHandler(ISAISLogger logger)
    {
        _logger = logger;
    }
}