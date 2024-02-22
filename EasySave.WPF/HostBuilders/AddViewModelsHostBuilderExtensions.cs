using EasySave.Models;
using EasySave.Services.Interfaces;
using EasySave.State.Navigators;
using EasySave.ViewModels;
using EasySave.ViewModels.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EasySave.HostBuilders
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

                //services.AddSingleton<IEasySaveViewModelFactory, EasySaveViewModelFactory>();

                services.AddSingleton<ViewModelDelegateRenavigator<BackupJobsListingViewModel>>();
                services.AddSingleton<ViewModelDelegateRenavigator<BackupJobCreationViewModel>>();
            });

            return host;
        }

        private static BackupJobsListingViewModel CreateBackupJobsListingViewModel(IServiceProvider services)
        {
            return new BackupJobsListingViewModel(
                services.GetRequiredService<IBackupJobService>(),
                services.GetRequiredService<ILogService>(),
                services.GetRequiredService<ViewModelDelegateRenavigator<BackupJobCreationViewModel>>());
        }

        private static BackupJobCreationViewModel CreateBackupJobCreationViewModel(IServiceProvider services)
        {
            return new BackupJobCreationViewModel(
                services.GetRequiredService<IBackupJobService>(),
                services.GetRequiredService<ILogService>(),
                services.GetRequiredService<ViewModelDelegateRenavigator<BackupJobsListingViewModel>>());
        }

    }
}
