using EasySave.Commands;
using EasySave.Commands.AppSettings;
using EasySave.Models;
using EasySave.Services.Interfaces;
using System.Windows;
using System.Windows.Input;

namespace EasySave.ViewModels
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

        public ICommand AddFileExtensionCommand { get; set; }
        public ICommand RemoveFileExtensionCommand { get; set; }

        public AppSettingsViewModel()
        {
            LoadAppSettingsCommand = new LoadAppSettingsCommand(this);
            SaveAppSettingsCommand = new SaveAppSettingsCommand(this);

            AddFileExtensionCommand = new AddFileExtensionCommand(this);
            RemoveFileExtensionCommand = new RemoveFileExtensionCommand(this);

            LoadAppSettingsCommand.Execute(this);
        }


    }
}
