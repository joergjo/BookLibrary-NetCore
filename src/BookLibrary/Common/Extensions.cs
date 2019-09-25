using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;

namespace BookLibrary.Common
{
    public static class Extensions
    {
        public static IServiceCollection AddMongoDatabase(
            this IServiceCollection services,
            Func<string> connectionStringFactory,
            string databaseName,
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
            if (databaseName == null)
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
