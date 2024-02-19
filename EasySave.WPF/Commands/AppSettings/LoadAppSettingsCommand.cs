using EasySave.Services;
using EasySave.Services.Interfaces;
using EasySave.ViewModels;
using System.Collections.ObjectModel;

namespace EasySave.Commands.AppSettings
{
    public class LoadAppSettingsCommand : AsyncCommandBase
    {
        private readonly AppSettingsViewModel _appSettingsViewModel;

        public LoadAppSettingsCommand(AppSettingsViewModel appSettingsViewModel)
        {
            _appSettingsViewModel = appSettingsViewModel;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _appSettingsViewModel.AppSettings = await AppSettingsService.LoadAppSettings();

            _appSettingsViewModel.FileExtensions = new ObservableCollection<string>(_appSettingsViewModel.AppSettings.AuthorizedExtensions);
        }
    }
}
