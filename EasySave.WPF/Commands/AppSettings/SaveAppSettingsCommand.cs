using EasySave.Domain.Services;
using EasySave.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.WPF.Commands
{
    public class SaveAppSettingsCommand : AsyncCommandBase
    {
        private readonly AppSettingsViewModel _appSettingsViewModel;
        private readonly IAppSettingsService _appSettingsService;

        public SaveAppSettingsCommand(AppSettingsViewModel appSettingsViewModel, IAppSettingsService appSettingsService)
        {
            _appSettingsViewModel = appSettingsViewModel;
            _appSettingsService = appSettingsService;
        }

        public override Task ExecuteAsync(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
