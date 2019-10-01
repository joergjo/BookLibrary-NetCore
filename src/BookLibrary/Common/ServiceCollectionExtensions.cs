using System;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace BookLibrary.Common
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
                var mongoUrl = new MongoUrl(connectionString);
                var client = new MongoClient(mongoUrl);
                var database = client.GetDatabase(databaseName);
                return database;
            });
            bsonMappingInitializer?.Invoke();

            return services;
        }
    }
}
