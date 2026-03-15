using Application.Utils.Logger;
using Infrastructure.Persistence.Context;
using Microsoft.Extensions.Logging;

namespace SAIS.Infrastructure.InternalServices.Logging;

public class SAISLogger<T> : ISAISLogger<T>
    where T : class
{
    private readonly ILogger<T> _logger;

    public SAISLogger(ILogger<T> logger)
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