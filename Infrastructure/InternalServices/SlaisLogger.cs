using Application.Utils.Logger;

using Microsoft.Extensions.Logging;

namespace SLAIS.Infrastructure.InternalServices.Logging;

public class SlaisLogger<T> : ISlaisLogger<T>
    where T : class
{
    private const string MessageTemplate = "{Message}";

    private const string MessageExceptionTemplate = MessageTemplate + " With The Exception: {Exception}";

    private readonly ILogger<T> _logger;

    public SlaisLogger(ILogger<T> logger)
    {
        _logger = logger;
    }

    public void LogInformation(string message)
    {
        _logger.LogInformation(MessageTemplate, message);
    }

    public void LogDebug(string message, Exception? exception)
    {
        _logger.LogDebug(MessageExceptionTemplate, message, exception);
    }

    public void LogWarning(string message)
    {
        _logger.LogWarning(MessageTemplate, message);
    }

    public void LogCritical(string message, Exception? exception)
    {
        _logger.LogCritical(MessageExceptionTemplate, message, exception);
    }

    public void LogError(string message, Exception? exception)
    {
        _logger.LogError(MessageExceptionTemplate, message, exception);
    }

}
