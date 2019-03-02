using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;

namespace BookLibrary
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseLamar()
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var secretsPath = Path.Combine(
                        hostingContext.HostingEnvironment.ContentRootPath,
                        "secrets");
                    config.AddKeyPerFile(secretsPath, optional: true);
                });

        private static void AddApplicationInsightsLogger(ILoggingBuilder loggingBuilder)
        {
            var loggingConfig = CreateLoggingConfiguration();
            string instrumentationKey = loggingConfig["ApplicationInsights:InstrumentationKey"];
            if (!string.IsNullOrEmpty(instrumentationKey))
            {
                loggingBuilder.AddApplicationInsights(instrumentationKey);
            }

            IConfiguration CreateLoggingConfiguration() =>
                new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddEnvironmentVariables()
                    .AddUserSecrets<Program>()
                    .Build();
        }
    }
}
