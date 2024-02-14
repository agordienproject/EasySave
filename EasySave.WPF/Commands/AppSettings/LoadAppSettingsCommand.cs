using EasySave.Domain.Services;
using EasySave.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.WPF.Commands
{
    public class LoadAppSettingsCommand : AsyncCommandBase
    {
        private readonly AppSettingsViewModel _appSettingsViewModel;
        private readonly IAppSettingsService _appSettingsService;

        public LoadAppSettingsCommand(AppSettingsViewModel appSettingsViewModel, IAppSettingsService appSettingsService)
        {
            _appSettingsViewModel = appSettingsViewModel;
            _appSettingsService = appSettingsService;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _appSettingsViewModel.AppSettings = await _appSettingsService.GetAppSettings();
        }
    }
}
