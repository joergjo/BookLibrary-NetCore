using BookLibrary.Api.Common;
using BookLibrary.Api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace BookLibrary
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add options as injectable component.
            services.AddOptions();

            // Add framework services.
            services.AddMvc(options =>
            {
                // Disable automatic fallback to JSON
                options.ReturnHttpNotAcceptable = true;

                // Honor browser's Accept header (e.g. Chrome) 
                options.RespectBrowserAcceptHeader = true;
            })
            .AddXmlDataContractSerializerFormatters();

            // Add application specific services
            services
                .Configure<MongoClientOptions>(Configuration.GetSection("MongoDB:DefaultConnection"))
                .AddMongoClient()
                .AddMongoCollection<Book>("books")
                .AddScoped<IBookRepository, BookRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseExceptionHandler(
                handler => handler.Run(
                    async context =>
                    {
                        string requestUri = $"{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
                        var errorData = new ErrorData
                        {
                            Message = "Unhandled exception. Use the error ID to debug the problem.",
                            DateTime = DateTimeOffset.Now,
                            RequestUri = new Uri(requestUri),
                            ErrorId = Guid.NewGuid()
                        };

                        if (env.IsDevelopment())
                        {
                            var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                            if (errorFeature?.Error != null)
                            {
                                errorData.Exception = errorFeature.Error;
                            }
                        }

                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(errorData));
                    }));

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddAzureWebAppDiagnostics();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();

            BsonMapping.Configure();
        }
    }
}
