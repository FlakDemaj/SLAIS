namespace Application.Utils.Logger;

public interface ISAISLogger<T>
    where T : class
{
    void LogInformation(string message);
    void LogWarning(string message);
    void LogError(string message, Exception? exception);
    void LogCritical(string message, Exception? exception);
    void LogDebug(string message, Exception? exception);
}