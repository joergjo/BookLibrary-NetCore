using BookLibrary.Common;
using BookLibrary.Models;
using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Diagnostics;
using System.Net.Mime;

namespace BookLibrary
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureContainer(ServiceRegistry services)
        {
            // Add framework services.
            services
                .AddRouting(options =>
                    options.ConstraintMap.Add("objectid", typeof(ObjectIdConstraint)))
                .AddMvc(options =>
                {
                    // Disable automatic fallback to JSON
                    options.ReturnHttpNotAcceptable = true;

                    // Honor browser's Accept header (e.g. Chrome) 
                    options.RespectBrowserAcceptHeader = true;
                })
                .AddXmlDataContractSerializerFormatters()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Add health monitoring
            services
                .AddHealthChecks()
                .AddMongoDb(Configuration.GetConnectionString("DefaultConnection"))
                .AddApplicationInsightsPublisher();

            // Add application specific services
            services
                // Register MongoDatabase instance
                .AddMongoDatabase(
                    connectionStringFactory: () => Configuration.GetConnectionString("DefaultConnection"),
                    databaseName: "library_database",
                    bsonMappingInitializer: () => BsonMapping.Configure())
                .AddScoped<IBookRepository, BookRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsProduction())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHealthChecks("/healthz/ready", new HealthCheckOptions
            {
                Predicate = reg => false
            });

            app.UseHealthChecks("/healthz/live", new HealthCheckOptions
            {
                Predicate = reg => false
            });

            app.UseExceptionHandler(
                new ExceptionHandlerOptions
                {
                    ExceptionHandler = async context =>
                    {
                        string requestId = Activity.Current?.Id ?? context.TraceIdentifier;
                        var errorInfo = new ErrorInformation
                        {
                            RequestId = requestId,
                            Message = $"Caught unhandled exception. Use request ID '{requestId}' to track the problem.",
                            DateTime = DateTimeOffset.UtcNow
                        };

                        var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (errorFeature?.Error != null)
                        {
                            var logger = context.RequestServices.GetRequiredService<ILogger<Startup>>();
                            logger.LogError(
                                ApplicationEvents.UnhandledException,
                                errorFeature.Error,
                                "Caught unhandled exception for request ID '{RequestId}'.",
                                requestId);
                        }

                        var settings = new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        };
                        string json = JsonConvert.SerializeObject(errorInfo, settings);
                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        await context.Response.WriteAsync(json);
                    }
                });

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
