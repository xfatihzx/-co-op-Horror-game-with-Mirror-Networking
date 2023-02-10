using UnityEngine;

public interface ILogger
{
    void Info(string message);
    void Log(string message);
    void Warning(string message);
    void Error(string message);
}
public enum LogLevel : byte
{
    None,
    Error,
    Warning,
    Log,
    Info
}
public static class Logger
{
    public static ILogger @do = new LoggerConsole();
#if UNITY_EDITOR
    public static LogLevel level = LogLevel.Info;
#elif UNITY_STANDALONE
    public static LogLevel level = LogLevel.None;
#elif UNITY_SERVER
    public static LogLevel level = LogLevel.Log;
#endif
    public static bool IsEnabled(LogLevel logLevel)
    {
        return (byte)level >= (byte)logLevel;
    }
}
public class LoggerConsole : ILogger
{
    public void Info(string message)
    {
        Debug.Log(message);
    }
    public void Log(string message)
    {
        Debug.Log(message);
    }
    public void Warning(string message)
    {
        Debug.LogWarning(message);
    }
    public void Error(string message)
    {
        Debug.LogError(message);
    }
}