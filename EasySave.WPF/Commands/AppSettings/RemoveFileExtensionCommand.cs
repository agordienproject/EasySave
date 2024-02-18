using EasySave.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Commands.AppSettings
{
    public class RemoveFileExtensionCommand : AsyncCommandBase
    {
        private readonly AppSettingsViewModel _appSettingsViewModel;

        public RemoveFileExtensionCommand(AppSettingsViewModel appSettingsViewModel)
        {
            _appSettingsViewModel = appSettingsViewModel;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            if (_appSettingsViewModel.AppSettings.AuthorizedExtensions.Contains(parameter.ToString()))
                _appSettingsViewModel.AppSettings.AuthorizedExtensions.Remove(parameter.ToString());
        }

    }
}
