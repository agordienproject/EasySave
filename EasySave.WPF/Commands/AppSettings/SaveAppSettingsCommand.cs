using EasySave.Services;
using EasySave.Services.Interfaces;
using EasySave.ViewModels;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;

namespace EasySave.Commands.AppSettings
{
    public class SaveAppSettingsCommand : AsyncCommandBase
    {
        private readonly AppSettingsViewModel _appSettingsViewModel;

        public SaveAppSettingsCommand(AppSettingsViewModel appSettingsViewModel)
        {
            _appSettingsViewModel = appSettingsViewModel;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _appSettingsViewModel.AppSettings.EncryptedExtensions = new List<string>(_appSettingsViewModel.EncryptedFileExtensions);
            _appSettingsViewModel.AppSettings.PrioritizedExtensions= new List<string>(_appSettingsViewModel.PrioritizedFileExtensions);

            await AppSettingsService.SaveAppSettings(_appSettingsViewModel.AppSettings);
        }
    }
}
