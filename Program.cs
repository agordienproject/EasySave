using EasySave.Controllers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using EasySave.Services;
using System.Globalization;
using EasySave.Services.Interfaces;

namespace EasySave;

class Program
{
    static async Task Main(string[] args)
    {
        // Configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppSettingsJson.GetApplicationExeDirectory())
            .AddJsonFile($"appsettings.json", false, true)
            .AddEnvironmentVariables()
            .Build();

        // Host
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<IConfiguration>(configuration);
                services.AddSingleton<App>();
                services.AddSingleton<IBackupController, BackupController>();
                services.AddSingleton<IBackupManager, BackupManager>();
                services.AddSingleton<IStateManager, StateManager>();
                services.AddSingleton<ILogManager, LogManager>();
            }).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                // Call Run command from App.cs
                var app = services.GetRequiredService<App>();
                await app.Run(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

        }

    }
}
