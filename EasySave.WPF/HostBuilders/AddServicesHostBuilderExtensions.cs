using EasySave.Services;
using EasySave.Services.Factories;
using EasySave.Services.Interfaces;
using EasySave.State.Navigators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EasySave.HostBuilders
{
    public static class AddServicesHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddSingleton<IFileServiceFactory, FileServiceFactory>();
                services.AddSingleton<IBackupJobService, BackupJobService>();
                services.AddSingleton<ILogService, LogService>();

                services.AddSingleton<INavigator, Navigator>();

                
            });

            return host;
        }
    }
}
