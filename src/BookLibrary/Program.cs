using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BookLibrary
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .ConfigureAppConfiguration((hostingContext, config) =>
                        {
                            var secretsPath = Path.Combine(
                                hostingContext.HostingEnvironment.ContentRootPath,
                                "secrets");
                            config.AddKeyPerFile(secretsPath, optional: true);
                        });
                });
    }
}
