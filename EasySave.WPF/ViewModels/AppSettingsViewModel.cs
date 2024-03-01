using EasySave.Commands;
using EasySave.Commands.AppSettings;
using EasySave.Models;
using EasySave.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Input;

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

        private ObservableCollection<string> _encryptedFileExtensions;
        public ObservableCollection<string> EncryptedFileExtensions
        {
            get { return _encryptedFileExtensions; }
            set
            {
                _encryptedFileExtensions = value;
                OnPropertyChanged(nameof(EncryptedFileExtensions));
            }
        }

        private ObservableCollection<string> _prioritizedFileExtensions;
        public ObservableCollection<string> PrioritizedFileExtensions
        {
            get { return _prioritizedFileExtensions; }
            set
            {
                _prioritizedFileExtensions = value;
                OnPropertyChanged(nameof(PrioritizedFileExtensions));
            }
        }

        private string _selectedEncryptedFileExtension;
        public string SelectedEncryptedFileExtension
        {
            get { return _selectedEncryptedFileExtension; }
            set
            {
                _selectedEncryptedFileExtension = value;
                OnPropertyChanged(nameof(SelectedEncryptedFileExtension));
            }
        }

        private string _selectedPrioritizedFileExtension;
        public string SelectedPrioritizedFileExtension
        {
            get { return _selectedPrioritizedFileExtension; }
            set
            {
                _selectedPrioritizedFileExtension = value;
                OnPropertyChanged(nameof(SelectedPrioritizedFileExtension));
            }
        }

        public ICommand LoadAppSettingsCommand { get; set; }
        public ICommand SaveAppSettingsCommand { get; set; }

        public ICommand AddEncryptedFileExtensionCommand { get; set; }
        public ICommand RemoveEncryptedFileExtensionCommand { get; set; }
        public ICommand AddPrioritizedFileExtensionCommand { get; set; }
        public ICommand RemovePrioritizedFileExtensionCommand { get; set; }

        public AppSettingsViewModel()
        {
            LoadAppSettingsCommand = new LoadAppSettingsCommand(this);
            SaveAppSettingsCommand = new SaveAppSettingsCommand(this);

            AddEncryptedFileExtensionCommand = new RelayCommand(AddEncryptedFileExtension);
            RemoveEncryptedFileExtensionCommand = new RelayCommand(RemoveEncryptedFileExtension);
            AddPrioritizedFileExtensionCommand = new RelayCommand(AddPrioritizedFileExtension);
            RemovePrioritizedFileExtensionCommand = new RelayCommand(RemovePrioritizedFileExtension);

            LoadAppSettingsCommand.Execute(this);
        }

        public void AddEncryptedFileExtension(object parameter)
        {
            if (!EncryptedFileExtensions.Contains(parameter.ToString()) && !String.IsNullOrWhiteSpace((string)parameter))
                EncryptedFileExtensions.Add(parameter.ToString());
        }

        public void RemoveEncryptedFileExtension(object parameter)
        {
            EncryptedFileExtensions.Remove(SelectedEncryptedFileExtension);
        }

        public void AddPrioritizedFileExtension(object parameter)
        {
            if (!PrioritizedFileExtensions.Contains(parameter.ToString()) && !String.IsNullOrWhiteSpace((string)parameter))
                PrioritizedFileExtensions.Add(parameter.ToString());
        }

        public void RemovePrioritizedFileExtension(object parameter)
        {
            PrioritizedFileExtensions.Remove(SelectedPrioritizedFileExtension);
        }
    }
}
