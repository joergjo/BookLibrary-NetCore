using BookLibrary.Common;
using BookLibrary.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
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
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services
                .AddMvc(options =>
                {
                    // Disable automatic fallback to JSON
                    options.ReturnHttpNotAcceptable = true;

                    // Honor browser's Accept header (e.g. Chrome) 
                    options.RespectBrowserAcceptHeader = true;
                })
                .AddXmlDataContractSerializerFormatters()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Add application specific services
            services
                // Register MongoDatabase instance
                .AddMongoDatabase(
                    () => Configuration.GetConnectionString("DefaultConnection"),
                    "library_database",
                    () => BsonMapping.Configure())
                .AddScoped<IBookRepository, BookRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddApplicationInsights(app.ApplicationServices, (s, l) =>
            {
                if (s.StartsWith("BookLibrary") && l >= LogLevel.Information)
                {
                    return true;
                }
                else if (l >= LogLevel.Warning)
                {
                    return true;
                }
                return false;
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

                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(json);
                    }
                });

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
