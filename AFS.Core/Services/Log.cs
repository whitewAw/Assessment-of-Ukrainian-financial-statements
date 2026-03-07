using Microsoft.Extensions.Logging;

namespace AFS.Core.Services;

/// <summary>
/// High-performance logging using source-generated LoggerMessage delegates.
/// AOT-safe and eliminates boxing/allocations for log parameters.
/// </summary>
internal static partial class Log
{
    [LoggerMessage(Level = LogLevel.Error, Message = "JSON deserialization failed: {Message}")]
    public static partial void JsonDeserializationFailed(ILogger logger, string message);

    [LoggerMessage(Level = LogLevel.Error, Message = "HTTP request failed: {Message}")]
    public static partial void HttpRequestFailed(ILogger logger, string message);

    [LoggerMessage(Level = LogLevel.Error, Message = "Invalid culture name: {Culture}")]
    public static partial void InvalidCultureName(ILogger logger, Exception ex, string culture);

    [LoggerMessage(Level = LogLevel.Error, Message = "Import failed: {Message}")]
    public static partial void ImportFailed(ILogger logger, string message);
}
