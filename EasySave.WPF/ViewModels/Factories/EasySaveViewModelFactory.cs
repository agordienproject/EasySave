using EasySave.State.Navigators;

namespace EasySave.ViewModels.Factories
{
    public class EasySaveViewModelFactory : IEasySaveViewModelFactory
    {
        private readonly CreateViewModel<BackupJobsListingViewModel> _createBackupJobsViewModel;
        private readonly CreateViewModel<AppSettingsViewModel> _createAppSettingsViewModel;

        public EasySaveViewModelFactory(CreateViewModel<BackupJobsListingViewModel> createBackupJobsViewModel, CreateViewModel<AppSettingsViewModel> createAppSettingsViewModel)
        {
            _createBackupJobsViewModel = createBackupJobsViewModel;
            _createAppSettingsViewModel = createAppSettingsViewModel;
        }

        public ViewModelBase CreateViewModel(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.BackupJobs:
                    return _createBackupJobsViewModel();
                case ViewType.AppSettings:
                    return _createAppSettingsViewModel();
                default:
                    throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType");
            }
        }
    }
}
