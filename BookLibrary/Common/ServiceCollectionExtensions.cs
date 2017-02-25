using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Authentication;

namespace BookLibrary.Api.Common
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoClient(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<MongoClient>(factory =>
            {
                var optionsAccessor = factory.GetRequiredService<IOptions<MongoClientOptions>>();
                var options = optionsAccessor.Value;

                var credentials = new List<MongoCredential>();
                if (!string.IsNullOrEmpty(options.Username))
                {
                    var identity = new MongoInternalIdentity(options.Database, options.Username);
                    var evidence = new PasswordEvidence(options.Password);
                    credentials.Add(new MongoCredential("SCRAM-SHA-1", identity, evidence));
                }

                var settings = new MongoClientSettings
                {
                    Credentials = credentials,
                    Server = new MongoServerAddress(options.Host, options.Port ?? 27017),
                    UseSsl = options.UseTransportSecurity ?? false,
                    SslSettings = new SslSettings
                    {
                        EnabledSslProtocols = SslProtocols.Tls12
                    }
                };

                var client = new MongoClient(settings);
                return client;
            });

            return services;
        }

        public static IServiceCollection AddMongoCollection<TDocument>(this IServiceCollection services, string collectionName)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (string.Empty == collectionName)
            {
                throw new ArgumentException("The collection name must not be an empty string.", nameof(collectionName));
            }

            services.AddSingleton<IMongoCollection<TDocument>>(factory =>
            {
                var optionsAccessor = factory.GetRequiredService<IOptions<MongoClientOptions>>();
                var options = optionsAccessor.Value;
                string documentName = typeof(TDocument).Name;

                var client = factory.GetRequiredService<MongoClient>();
                var database = client.GetDatabase(options.Database);
                var collection = database.GetCollection<TDocument>(collectionName ?? documentName.ToLowerInvariant());
                return collection;
            });

            return services;
        }
    }
}
