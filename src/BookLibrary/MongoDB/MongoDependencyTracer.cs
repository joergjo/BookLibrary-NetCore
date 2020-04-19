using System;
using System.Collections.Concurrent;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Logging;
using MongoDB.Driver.Core.Events;

namespace BookLibrary.MongoDB
{
    public class MongoDependencyTracer
    {
        private readonly ConcurrentDictionary<int, DependencyTelemetry> _operations;
        private readonly TelemetryClient _telemetryClient;
        private readonly ILogger _logger;

        private const string DatabaseType = "monogdb";

        public MongoDependencyTracer(TelemetryClient telemetryClient, ILogger<MongoDependencyTracer> logger)
        {
            _operations = new ConcurrentDictionary<int, DependencyTelemetry>();
            _telemetryClient = telemetryClient;
            _logger = logger;
        }

        public void OnCommandStarted(CommandStartedEvent @event)
        {
            var telemetry = NewDependencyTelemetry(@event);
            if (_operations.TryAdd(@event.RequestId, telemetry))
            {
                _logger.LogOperationAdded(telemetry.Id, @event.RequestId);
            }
            else
            {
                _logger.LogErrorAddingOperation(telemetry.Id, @event.RequestId);
            }
        }

        public void OnCommandSucceeded(CommandSucceededEvent @event)
        {
            if (_operations.TryRemove(@event.RequestId, out var telemetry))
            {
                CompleteDependencyTelemetry(
                    telemetry,
                    @event.Duration,
                    true,
                    ("mongo-reply", @event.Reply.ToString()));
                _telemetryClient.TrackDependency(telemetry);
                _logger.LogOperationRemoved(telemetry.Id, @event.RequestId);
            }
            else
            {
                _logger.LogErrorRemovingOperation(@event.RequestId);
            }
        }

        public void OnCommandFailed(CommandFailedEvent @event)
        {
            if (_operations.TryRemove(@event.RequestId, out var telemetry))
            {
                CompleteDependencyTelemetry(
                    telemetry,
                    @event.Duration,
                    false,
                    ("mongo-failure", @event.Failure.ToString()));
                _telemetryClient.TrackDependency(telemetry);
                _logger.LogOperationRemoved(telemetry.Id, @event.RequestId);
            }
            else
            {
                _logger.LogErrorRemovingOperation(@event.RequestId);
            }
        }

        private static DependencyTelemetry NewDependencyTelemetry(CommandStartedEvent @event) =>
            new DependencyTelemetry
            {
                Type = DatabaseType,
                Timestamp = DateTimeOffset.UtcNow,
                Name = @event.CommandName,
                Data = @event.Command.ToString(),
                Target = @event.ConnectionId.ServerId.EndPoint.ToString()
            };

        private static void CompleteDependencyTelemetry(
            DependencyTelemetry telemetry,
            TimeSpan duration,
            bool isSuccess,
            params (string, string)[] properties)
        {
            telemetry.Success = isSuccess;
            telemetry.Duration = duration;
            foreach ((string key, string value) in properties)
            {
                telemetry.Properties.Add(key, value);
            }
        }
    }
}
