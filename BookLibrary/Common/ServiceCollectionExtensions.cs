using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;

namespace BookLibrary.Api.Common
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoClient(this IServiceCollection services, string connectionString)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (connectionString == string.Empty)
            {
                throw new ArgumentException("The MongoDB connection string must not be an empty string.", nameof(connectionString));
            }

            var mongoUrl = MongoUrl.Create(connectionString);
            services.AddSingleton<MongoUrl>(mongoUrl);
            services.AddSingleton<MongoClient>(new MongoClient(mongoUrl));

            return services;
        }

        public static IServiceCollection AddMongoCollection<TDocument>(this IServiceCollection services, string collectionName)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (collectionName == null)
            {
                throw new ArgumentNullException(nameof(collectionName));
            }

            if (collectionName == string.Empty)
            {
                throw new ArgumentException("The collection name must not be an empty string.", nameof(collectionName));
            }

            services.AddSingleton<IMongoCollection<TDocument>>(factory =>
            {
                var mongoUrl = factory.GetRequiredService<MongoUrl>();
                string databaseName = mongoUrl.DatabaseName;

                var client = factory.GetRequiredService<MongoClient>();
                var database = client.GetDatabase(databaseName);
                var collection = database.GetCollection<TDocument>(collectionName ?? typeof(TDocument).Name.ToLowerInvariant());
                return collection;
            });

            return services;
        }
    }
}
