using EasySave.DataAccess.Services;
using EasySave.DataAccess.Services.Factories;
using EasySave.Domain.Services;
using EasySave.WPF.State.Navigators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.WPF.HostBuilders
{
    public static class AddServicesHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddSingleton<IAppSettingsService, AppSettingsService>();
                services.AddSingleton<IFileServiceFactory, FileServiceFactory>();
                services.AddSingleton<IBackupJobService, BackupJobService>();
                services.AddSingleton<IStateService, StateService>();
                services.AddSingleton<ILogService, LogService>();

                services.AddSingleton<INavigator, Navigator>();
            });

            return host;
        }
    }
}
