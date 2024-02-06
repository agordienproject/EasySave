using EasySave.Controllers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using EasySave.Services;
using System.Globalization;

namespace EasySave;

class Program
{
    static async Task Main(string[] args)
    {
        // Configuration
        //var builder = new ConfigurationBuilder()
        //    .SetBasePath(ApplicationExeDirectory())
        //    .AddJsonFile("appSettings.json", false, true)
        //    .AddEnvironmentVariables()
        //    .Build();

        // Host
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<App>();
                services.AddSingleton<IBackupController, BackupController>();
            }).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var app = services.GetRequiredService<App>();
                await app.Run(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<App>();
                    services.AddSingleton<IBackupController, BackupController>();
                    //services.AddSingleton<IConfiguration>(configuration);
                });

        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                //.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", false)
                .AddEnvironmentVariables();
        }
    }
}
