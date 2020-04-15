using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace BookLibrary.Mongo
{
    public static class LoggerExtensions
    {
        private static readonly Action<ILogger, ActivitySpanId, int, Exception?> _spanAddedToCache =
            LoggerMessage.Define<ActivitySpanId, int>(
                LogLevel.Debug,
                TracingEvents.SpanAddedToCache,
                "Added new span '{SpanId}' for MongoDB request '{RequestId}' to cache.");

        private static readonly Action<ILogger, ActivitySpanId, int, Exception?> _spanRemovedFromCache =
            LoggerMessage.Define<ActivitySpanId, int>(
                LogLevel.Debug,
                TracingEvents.SpanRemovedFromCache,
                "Removed completed span '{SpanId}' for MongoDB request '{RequestId}' from cache.");

        private static readonly Action<ILogger, ActivitySpanId, int, Exception?> _errorAddingSpanToCache =
            LoggerMessage.Define<ActivitySpanId, int>(
                LogLevel.Warning,
                TracingEvents.ErrorAddingSpanToCache,
                "Failed to add new span '{SpanId}' for MongoDB request '{RequestId}' to cache.");

        private static readonly Action<ILogger, int, Exception?> _errorRemovingSpanFromCache =
            LoggerMessage.Define<int>(
                LogLevel.Warning,
                TracingEvents.ErrorRemovingSpanFromCache,
                "Failed to remove completed span for MongoDB request '{RequestId}' from cache.");

        public static void LogSpanAdded(this ILogger logger, ActivitySpanId spanId, int requestId)
        {
            _spanAddedToCache(logger, spanId, requestId, null);
        }

        public static void LogSpanRemoved(this ILogger logger, ActivitySpanId spanId, int requestId)
        {
            _spanRemovedFromCache(logger, spanId, requestId, null);
        }

        public static void LogErrorAddingSpan(this ILogger logger, ActivitySpanId spanId, int requestId)
        {
            _errorAddingSpanToCache(logger, spanId, requestId, null);
        }

        public static void LogErrorRemovingSpan(this ILogger logger, int requestId)
        {
            _errorRemovingSpanFromCache(logger, requestId, null);
        }
    }
}
