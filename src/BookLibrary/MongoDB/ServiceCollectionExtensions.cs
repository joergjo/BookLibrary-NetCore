using System;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace BookLibrary.MongoDB
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDatabase(
            this IServiceCollection services,
            Func<string> connectionStringFactory,
            string databaseName,
            Action? bsonMappingInitializer = default)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (connectionStringFactory is null)
            {
                throw new ArgumentNullException(nameof(connectionStringFactory));
            }
            if (databaseName is null)
            {
                throw new ArgumentNullException(nameof(databaseName));
            }
            if (databaseName == string.Empty)
            {
                new ArgumentOutOfRangeException(nameof(databaseName));
            }

            services.AddSingleton(serviceProvider =>
            {
                string connectionString = connectionStringFactory();
                var clientSettings = MongoClientSettings.FromConnectionString(connectionString);
                var eventTracer = serviceProvider.GetRequiredService<MongoDependencyTracer>();
                clientSettings.AddApplicationInsightsDependencyTracing(eventTracer);
                var client = new MongoClient(clientSettings);
                var database = client.GetDatabase(databaseName);
                return database;
            });
            bsonMappingInitializer?.Invoke();

            return services;
        }
    }
}
