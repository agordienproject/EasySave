using EasySave.Services;
using EasySave.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Commands.AppSettings
{
    public class AddFileExtensionCommand : AsyncCommandBase
    {
        private readonly AppSettingsViewModel _appSettingsViewModel;

        public AddFileExtensionCommand(AppSettingsViewModel appSettingsViewModel)
        {
            _appSettingsViewModel = appSettingsViewModel;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            if (!_appSettingsViewModel.AppSettings.AuthorizedExtensions.Contains(parameter.ToString()))
                _appSettingsViewModel.AppSettings.AuthorizedExtensions.Add(parameter.ToString());
        }

    }
}
