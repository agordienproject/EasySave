using EasySave.WPF.State.Navigators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.WPF.ViewModels.Factories
{
    public class EasySaveViewModelFactory : IEasySaveViewModelFactory
    {
        private readonly CreateViewModel<BackupJobsViewModel> _createBackupJobsViewModel;
        private readonly CreateViewModel<AppSettingsViewModel> _createAppSettingsViewModel;

        public EasySaveViewModelFactory(CreateViewModel<BackupJobsViewModel> createBackupJobsViewModel, CreateViewModel<AppSettingsViewModel> createAppSettingsViewModel)
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
