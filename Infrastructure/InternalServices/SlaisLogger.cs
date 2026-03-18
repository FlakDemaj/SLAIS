using Application.Utils.Logger;

using Microsoft.Extensions.Logging;

namespace SLAIS.Infrastructure.InternalServices.Logging;

public class SlaisLogger<T> : ISlaisLogger<T>
    where T : class
{
    private const string MessageTemplate = "{Message}";

    private readonly ILogger<T> _logger;

    public SlaisLogger(ILogger<T> logger)
    {
        _logger = logger;
    }

    public void LogInformation(string message)
    {
        CheckEnabling(LogLevel.Debug);
        _logger.LogInformation(MessageTemplate, message);
    }

    public void LogDebug(string message, Exception? exception)
    {
        CheckEnabling(LogLevel.Debug);
        _logger.LogDebug(MessageTemplate, message);
    }

    public void LogWarning(string message)
    {
        CheckEnabling(LogLevel.Warning);
        _logger.LogWarning(MessageTemplate, message);
    }

    public void LogCritical(string message, Exception? exception)
    {
        CheckEnabling(LogLevel.Critical);
        _logger.LogCritical(MessageTemplate, message);
    }

    public void LogError(string message, Exception? exception)
    {
        CheckEnabling(LogLevel.Error);
        _logger.LogError(MessageTemplate, message);
    }

    private void CheckEnabling(LogLevel logLevel)
    {
        if (!_logger.IsEnabled(logLevel))
        {
            return;
        }
    }

}
