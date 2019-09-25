using BookLibrary.Common;
using BookLibrary.Models;
using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                .AddControllers(options =>
                {
                    // Disable automatic fallback to JSON
                    options.ReturnHttpNotAcceptable = true;

                    // Honor browser's Accept header (e.g. Chrome) 
                    options.RespectBrowserAcceptHeader = true;
                })
                .AddXmlDataContractSerializerFormatters();

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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment _)
        {
            app.UseExceptionHandler("/error");

            app.UseHealthChecks("/healthz/ready", new HealthCheckOptions
            {
                Predicate = reg => false
            });

            app.UseHealthChecks("/healthz/live", new HealthCheckOptions
            {
                Predicate = reg => false
            });

            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
