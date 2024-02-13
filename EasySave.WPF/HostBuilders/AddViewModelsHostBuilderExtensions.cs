using EasySave.WPF.ViewModels;
using EasySave.WPF.ViewModels.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.WPF.HostBuilders
{
    public static class AddViewModelsHostBuilderExtensions
    {
        public static IHostBuilder AddViewModels(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddTransient<MainViewModel>();
                services.AddTransient<AppSettingsViewModel>();
                services.AddTransient<BackupJobsViewModel>();

                services.AddSingleton<CreateViewModel<BackupJobsViewModel>>(services => () => services.GetRequiredService<BackupJobsViewModel>());
                services.AddSingleton<CreateViewModel<AppSettingsViewModel>>(services => () => services.GetRequiredService<AppSettingsViewModel>());

                services.AddSingleton<IEasySaveViewModelFactory, EasySaveViewModelFactory>();
            });

            return host;
        }

        
    }
}
