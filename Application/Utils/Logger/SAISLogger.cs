using Application.Utils.Logger;
using Microsoft.Extensions.Logging;

namespace SAIS.Infrastructure.InternalServices.Logging;

public class SAISLogger : ISAISLogger
{
    private readonly ILogger<SAISLogger> _logger;

    public SAISLogger(ILogger<SAISLogger> logger)
    {
        _logger = logger;
    }

    public void LogInformation(string message)
    {
        _logger.LogInformation("{Message}", message);
    }
    
    public void LogDebug(string message, Exception? exception)
    {
        _logger.LogDebug("{Message}", message);
    }

    public void LogWarning(string message)
    {
        _logger.LogWarning("{Message}", message);
    }

    public void LogCritical(string message, Exception? exception)
    {
        _logger.LogCritical("{Message}", message); 
    }
    
    public void LogError(string message, Exception? exception)
    {
        _logger.LogError("{Message}", message);

    }
}