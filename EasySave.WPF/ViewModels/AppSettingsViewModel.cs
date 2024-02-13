using EasySave.Domain.Models;
using EasySave.Domain.Services;
using EasySave.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasySave.WPF.ViewModels
{
    public class AppSettingsViewModel : ViewModelBase
    {
        private AppSettings _appSettings;
        public AppSettings? AppSettings 
        { 
            get { return _appSettings; }
            set 
            { 
                _appSettings = value;
                OnPropertyChanged(nameof(AppSettings));
            }
        }

        public ICommand LoadAppSettingsCommand { get; set; }
        public ICommand SaveAppSettingsCommand { get; set; }

        public AppSettingsViewModel(IAppSettingsService appSettingsService)
        {
            LoadAppSettingsCommand = new LoadAppSettingsCommand(this, appSettingsService);
            SaveAppSettingsCommand = new SaveAppSettingsCommand(this, appSettingsService);
        }


    }
}
