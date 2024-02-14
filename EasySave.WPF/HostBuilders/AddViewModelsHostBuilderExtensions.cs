using EasySave.Domain.Services;
using EasySave.WPF.State.Navigators;
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
                services.AddTransient(CreateBackupJobsListingViewModel);
                services.AddTransient(CreateBackupJobCreationViewModel);

                services.AddSingleton<CreateViewModel<AppSettingsViewModel>>(services => () => services.GetRequiredService<AppSettingsViewModel>());
                services.AddSingleton<CreateViewModel<BackupJobsListingViewModel>>(services => () => services.GetRequiredService<BackupJobsListingViewModel>());
                services.AddSingleton<CreateViewModel<BackupJobCreationViewModel>>(services => () => services.GetRequiredService<BackupJobCreationViewModel>());

                services.AddSingleton<IEasySaveViewModelFactory, EasySaveViewModelFactory>();

                services.AddSingleton<ViewModelDelegateRenavigator<BackupJobsListingViewModel>>();
                services.AddSingleton<ViewModelDelegateRenavigator<BackupJobCreationViewModel>>();
            });

            return host;
        }

        private static BackupJobsListingViewModel CreateBackupJobsListingViewModel(IServiceProvider services)
        {
            return new BackupJobsListingViewModel(
                services.GetRequiredService<IBackupJobService>(),
                services.GetRequiredService<ViewModelDelegateRenavigator<BackupJobCreationViewModel>>());
        }

        private static BackupJobCreationViewModel CreateBackupJobCreationViewModel(IServiceProvider services)
        {
            return new BackupJobCreationViewModel(
                services.GetRequiredService<IBackupJobService>(),
                services.GetRequiredService<ViewModelDelegateRenavigator<BackupJobsListingViewModel>>());
        }

    }
}
