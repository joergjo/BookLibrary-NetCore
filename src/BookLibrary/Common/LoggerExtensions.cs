using System;
using Microsoft.Extensions.Logging;

namespace BookLibrary.Common
{
    public static class LoggerExtensions
    {
        private static readonly Action<ILogger, int, Exception?> _libraryQueried =
            LoggerMessage.Define<int>(
                LogLevel.Information,
                ApplicationEvents.LibraryQueried,
                "Retrieved {Count} books.");

        private static readonly Action<ILogger, string, Exception?> _bookQueried =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                ApplicationEvents.BookQueried,
                "Retrieved book '{BookId}'.");

        private static readonly Action<ILogger, string, Exception?> _bookNotFound =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                ApplicationEvents.BookNotFound,
                "Failed to retrieve book '{BookId}'.");

        private static readonly Action<ILogger, string, Exception?> _bookCreated =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                ApplicationEvents.BookCreated,
                "Added book with id '{BookId}'.");

        private static readonly Action<ILogger, string, Exception?> _bookUpdated =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                ApplicationEvents.BookUpdated,
                "Updated book with id '{BookId}'.");

        private static readonly Action<ILogger, string, Exception?> _bookDeleted =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                ApplicationEvents.BookDeleted,
                "Removed book with id '{BookId}'.");

        private static readonly Action<ILogger, string, Exception?> _unhandledExceptionOccurred =
            LoggerMessage.Define<string>(
                LogLevel.Error,
                ApplicationEvents.UnhandledExceptionOccurred,
                "Caught unhandled exception for request ID '{RequestId}'.");

        public static void LogLibraryQueried(this ILogger logger, int count)
        {
            _libraryQueried(logger, count, null);
        }

        public static void LogBookQueried(this ILogger logger, string id)
        {
            _bookQueried(logger, id, null);
        }

        public static void LogBookCreated(this ILogger logger, string id)
        {
            _bookCreated(logger, id, null);
        }

        public static void LogBookUpdated(this ILogger logger, string id)
        {
            _bookUpdated(logger, id, null);
        }

        public static void LogBookDeleted(this ILogger logger, string id)
        {
            _bookDeleted(logger, id, null);
        }

        public static void LogBookNotFound(this ILogger logger, string id)
        {
            _bookNotFound(logger, id, null);
        }

        public static void LogUnhandledException(this ILogger logger, string requestId, Exception error)
        {
            _unhandledExceptionOccurred(logger, requestId, error);
        }
    }
}
