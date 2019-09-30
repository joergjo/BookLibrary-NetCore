using BookLibrary.Common;
using BookLibrary.Models;
using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BookLibrary
{
    public class Startup
    {
        private const string DatabaseName = "library_database";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureContainer(ServiceRegistry services)
        {
            services.AddApplicationInsightsTelemetry();

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
                .AddMongoDb(
                    mongodbConnectionString: Configuration.GetConnectionString("DefaultConnection"),
                    name: "mongodb_health_check",
                    mongoDatabaseName: DatabaseName,
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new[] { "ready" })
                .AddApplicationInsightsPublisher();

            // Add application specific services
            services
                // Register MongoDatabase instance
                .AddMongoDatabase(
                    connectionStringFactory: () => Configuration.GetConnectionString("DefaultConnection"),
                    databaseName: DatabaseName,
                    bsonMappingInitializer: () => BsonMapping.Configure())
                .AddScoped<ILibraryService, MongoLibraryService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment _)
        {
            app.UseExceptionHandler("/error");
            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/healthz/live", new HealthCheckOptions
                {
                    Predicate = _ => false
                });
                endpoints.MapHealthChecks("/healthz/ready", new HealthCheckOptions
                {
                    Predicate = reg => reg.Tags.Contains("ready")
                });
                endpoints.MapControllers();
            });
        }
    }
}
