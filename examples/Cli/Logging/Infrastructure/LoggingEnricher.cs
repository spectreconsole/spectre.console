using Serilog.Core;
using Serilog.Events;

namespace Logging.Infrastructure;

internal class LoggingEnricher : ILogEventEnricher
{
    private string _cachedLogFilePath;
    private LogEventProperty _cachedLogFilePathProperty;

    // this path and level will be set by the LogInterceptor.cs after parsing the settings
    public static string Path = string.Empty;

    public const string LogFilePathPropertyName = "LogFilePath";

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        // the settings might not have a path or we might not be within a command in which case
        // we won't have the setting so a default value for the log file will be required
        LogEventProperty logFilePathProperty;

        if (_cachedLogFilePathProperty != null && Path.Equals(_cachedLogFilePath))
        {
            // Path hasn't changed, so let's use the cached property
            logFilePathProperty = _cachedLogFilePathProperty;
        }
        else
        {
            // We've got a new path for the log. Let's create a new property
            // and cache it for future log events to use
            _cachedLogFilePath = Path;
            _cachedLogFilePathProperty = logFilePathProperty = propertyFactory.CreateProperty(LogFilePathPropertyName, Path);
        }

        logEvent.AddPropertyIfAbsent(logFilePathProperty);
    }
}
