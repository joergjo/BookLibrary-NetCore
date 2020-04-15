using System;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace BookLibrary.Mongo
{
    public static class MongoDriverExtensions
    {
        public static MongoClientSettings AddOpenTracing(
            this MongoClientSettings clientSettings,
            MongoEventTracer eventTracer)
        {
            if (clientSettings is null)
            {
                throw new ArgumentNullException(nameof(clientSettings));
            }

            if (eventTracer is null)
            {
                throw new ArgumentNullException(nameof(eventTracer));
            }

            clientSettings.ClusterConfigurator =
                builder =>
                {
                    builder.Subscribe<CommandStartedEvent>(eventTracer.OnCommandStarted);
                    builder.Subscribe<CommandSucceededEvent>(eventTracer.OnCommandSucceeded);
                    builder.Subscribe<CommandFailedEvent>(eventTracer.OnCommandFailed);
                };
            return clientSettings;
        }
    }
}
