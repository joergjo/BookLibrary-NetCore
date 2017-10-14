using BookLibrary.Api.Common;
using BookLibrary.Api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;

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
                .AddXmlDataContractSerializerFormatters();

            // Add application specific services
            services
                // Register MongoDatabase instance
                .AddMongoDatabase(
                    () => Configuration.GetConnectionString("DefaultConnection"),
                    () => BsonMapping.Configure())
                .AddScoped<IBookRepository, BookRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
