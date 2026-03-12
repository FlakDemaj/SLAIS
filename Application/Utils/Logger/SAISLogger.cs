using Application.Utils.Logger;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SAIS.Infrastructure.InternalServices.Logging;

public class SAISLogger : ISAISLogger
{
    private readonly ILogger<SAISLogger> _logger;
    private readonly IHub _sentryHub;

    public SAISLogger(ILogger<SAISLogger> logger, IHub sentryHub)
    {
        _logger = logger;
        _sentryHub = sentryHub;
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
        
        LogEventToSentry(
            message,
            "Warning",
            BreadcrumbLevel.Warning,
            SentryLevel.Warning,
            null);
    }

    public void LogCritical(string message, Exception? exception)
    {
        _logger.LogCritical("{Message}", message);
        
        LogEventToSentry(
            message, 
            "Critival",
            BreadcrumbLevel.Fatal,
            SentryLevel.Fatal,
            exception);
    }
    
    public void LogError(string message, Exception? exception)
    {
        _logger.LogError("{Message}", message);
        
        LogEventToSentry(message,
            "Error",
            BreadcrumbLevel.Error,
            SentryLevel.Error,
            exception);

    }

    private void LogEventToSentry(
        string message,
        string category,
        BreadcrumbLevel breadcrumbLevel,
        SentryLevel level,
        Exception? exception)
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != Environments.Production)
        {
            return;
        }
        
        _sentryHub.AddBreadcrumb(
            message: message,
            category: category,
            level: breadcrumbLevel
            );

        if (exception != null)
        {
            _sentryHub.CaptureException(exception);
        }

        var sentryEvent = new SentryEvent
        {
            Level = level,
            Message = new SentryMessage
            {
                Message = message
            }
        };
        
        _sentryHub.CaptureEvent(sentryEvent);

    }
}