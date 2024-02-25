using EasySave.Commands;
using EasySave.Services.Interfaces;
using EasySave.State.Navigators;
using EasySave.ViewModels;
using System.Windows.Input;

namespace EasySave.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigator _navigator;

        public ViewModelBase CurrentViewModel => _navigator.CurrentViewModel;

        public BackupJobsListingViewModel BackupJobsListingViewModel;
        public AppSettingsViewModel AppSettingsViewModel;

        public ICommand UpdateCurrentViewModelCommand { get; }

        public MainViewModel(INavigator navigator, 
            BackupJobsListingViewModel backupJobsListingViewModel,
            AppSettingsViewModel appSettingsViewModel)
        {
            _navigator = navigator;

            _navigator.StateChanged += Navigator_StateChanged;

            BackupJobsListingViewModel = backupJobsListingViewModel;
            AppSettingsViewModel = appSettingsViewModel;

            UpdateCurrentViewModelCommand = new RelayCommand(UpdateCurrentViewModel);
            UpdateCurrentViewModelCommand.Execute(ViewType.BackupJobs);
        }

        private void Navigator_StateChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));

            if (CurrentViewModel is BackupJobsListingViewModel)
            {
                BackupJobsListingViewModel.LoadBackupJobsCommand.Execute(BackupJobsListingViewModel);
            }
        }

        public override void Dispose()
        {
            _navigator.StateChanged -= Navigator_StateChanged;

            base.Dispose();
        }

        public void UpdateCurrentViewModel(object parameter)
        {
            if (parameter is ViewType)
            {
                ViewType viewType = (ViewType)parameter;

                switch (viewType)
                {
                    case ViewType.BackupJobs:
                        _navigator.CurrentViewModel = BackupJobsListingViewModel;
                        break;
                    case ViewType.AppSettings:
                        _navigator.CurrentViewModel = AppSettingsViewModel;
                        break;
                    default:
                        throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType");
                }
            }
        }
    }
}
