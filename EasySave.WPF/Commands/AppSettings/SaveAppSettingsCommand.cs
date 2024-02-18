using EasySave.Services;
using EasySave.Services.Interfaces;
using EasySave.ViewModels;
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
            await AppSettingsService.SaveAppSettings(_appSettingsViewModel.AppSettings);
        }
    }
}
