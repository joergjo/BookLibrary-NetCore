using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using MongoDB.Driver.Core.Events;
using OpenTelemetry.Trace;
using OpenTelemetry.Trace.Configuration;

namespace BookLibrary.Mongo
{
    public class MongoEventTracer
    {
        private readonly ConcurrentDictionary<int, TelemetrySpan> _spans;
        private readonly Tracer _tracer;
        private readonly ILogger _logger;
        private const string TracerName = "mongo-events";
        private const string DatabaseType = "monogdb";

        public MongoEventTracer(TracerFactory tracerFactory, ILogger<MongoEventTracer> logger)
        {
            _spans = new ConcurrentDictionary<int, TelemetrySpan>();
            _tracer = tracerFactory.GetTracer(TracerName);
            _logger = logger;
        }

        public void OnCommandStarted(CommandStartedEvent @event)
        {
            var span = NewSpan(@event);
            if (_spans.TryAdd(@event.RequestId, span))
            {
                _logger.LogSpanAdded(span.Context.SpanId, @event.RequestId);
            }
            else
            {
                _logger.LogErrorAddingSpan(span.Context.SpanId, @event.RequestId);
            }
        }

        public void OnCommandSucceeded(CommandSucceededEvent @event)
        {
            if (_spans.TryRemove(@event.RequestId, out var span))
            {
                span.SetAttribute("mongo-reply", @event.Reply.ToString());
                span.End();
                _logger.LogSpanRemoved(span.Context.SpanId, @event.RequestId);
            }
            else
            {
                _logger.LogErrorRemovingSpan(@event.RequestId);
            }
        }

        public void OnCommandFailed(CommandFailedEvent @event)
        {
            if (_spans.TryRemove(@event.RequestId, out var span))
            {
                span.SetAttribute("mongo-failure", @event.Failure.ToString());
                span.End();
                _logger.LogSpanRemoved(span.Context.SpanId, @event.RequestId);
            }
            else
            {
                _logger.LogErrorRemovingSpan(@event.RequestId);
            }
        }

        private TelemetrySpan NewSpan(CommandStartedEvent @event)
        {
            var span = _tracer
                .StartSpan(@event.CommandName, SpanKind.Client)
                .PutDatabaseTypeAttribute(DatabaseType)
                .PutDatabaseInstanceAttribute(@event.DatabaseNamespace.DatabaseName)
                .PutDatabaseStatementAttribute(@event.Command.ToString());
            span.SetAttribute("db.host", @event.ConnectionId.ToString());
            return span;
        }
    }
}
