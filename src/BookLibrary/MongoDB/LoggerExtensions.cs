using System;
using Microsoft.Extensions.Logging;

namespace BookLibrary.MongoDB
{
    public static class LoggerExtensions
    {
        private static readonly Action<ILogger, string, int, Exception?> _telemetryAddedToCache =
            LoggerMessage.Define<string, int>(
                LogLevel.Debug,
                TracingEvents.TelemetryAddedToCache,
                "Added new operation '{TelemetryId}' for MongoDB request '{RequestId}' to cache.");

        private static readonly Action<ILogger, string, int, Exception?> _telemetryRemovedFromCache =
            LoggerMessage.Define<string, int>(
                LogLevel.Debug,
                TracingEvents.TelemetryRemovedFromCache,
                "Removed completed operation '{TelemetryId}' for MongoDB request '{RequestId}' from cache.");

        private static readonly Action<ILogger, string, int, Exception?> _errorAddingTelemetryToCache =
            LoggerMessage.Define<string, int>(
                LogLevel.Warning,
                TracingEvents.ErrorAddingTelemetryToCache,
                "Failed to add new operation '{TelemetryId}' for MongoDB request '{RequestId}' to cache.");

        private static readonly Action<ILogger, int, Exception?> _errorRemovingTelemetryFromCache =
            LoggerMessage.Define<int>(
                LogLevel.Warning,
                TracingEvents.ErrorRemovingTelemetryFromCache,
                "Failed to remove completed operation for MongoDB request '{RequestId}' from cache.");

        public static void LogOperationAdded(this ILogger logger, string telemetryId, int requestId)
        {
            _telemetryAddedToCache(logger, telemetryId, requestId, null);
        }

        public static void LogOperationRemoved(this ILogger logger, string telemetryId, int requestId)
        {
            _telemetryRemovedFromCache(logger, telemetryId, requestId, null);
        }

        public static void LogErrorAddingOperation(this ILogger logger, string telemetryId, int requestId)
        {
            _errorAddingTelemetryToCache(logger, telemetryId, requestId, null);
        }

        public static void LogErrorRemovingOperation(this ILogger logger, int requestId)
        {
            _errorRemovingTelemetryFromCache(logger, requestId, null);
        }
    }
}
