using SAIS.Domain.Commom.Logger;

namespace SAIS.Infrastructure.InternalServices.Logging;

public static class StaticLogger
{
    private static ISAISLogger? _logger;

    public static void Initialize(ISAISLogger logger)
    {
        if (_logger != null)
        {
            throw new InvalidOperationException("Logger is already initialized");
        }
        
        _logger = logger;
    }

    private static ISAISLogger Logger
    {
        get
        {
            if (_logger == null)
            {
                throw new InvalidOperationException("Logger is not initialized");
            }
        
            return _logger;
            
        }
    }

    public static void LogInformation(string message)
    {
        Logger.LogInformation(message);
    }

    public static void LogWarning(string message)
    {
        Logger.LogWarning(message);
    }

    public static void LogDebug(string message, Exception? exception)
    {
        Logger.LogDebug(message, exception);
    }

    public static void LogError(string message, Exception? exception)
    {
        Logger.LogError(message, exception);
    }

    public static void LogCritical(string message, Exception? exception)
    {
        Logger.LogCritical(message, exception);
    }
}