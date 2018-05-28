using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;

namespace BookLibrary.Common
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDatabase(
            this IServiceCollection services,
            Func<string> connectionStringFactory,
            string databaseName = null,
            Action bsonMappingInitializer = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (connectionStringFactory == null)
            {
                throw new ArgumentNullException(nameof(connectionStringFactory));
            }

            services.AddSingleton(serviceProvider =>
            {
                string connectionString = connectionStringFactory();
                var mongoUrl = new MongoUrl(connectionString);
                var client = new MongoClient(mongoUrl);
                var database = client.GetDatabase(databaseName ?? mongoUrl.DatabaseName);
                return database;
            });
            bsonMappingInitializer?.Invoke();

            return services;
        }
    }
}
