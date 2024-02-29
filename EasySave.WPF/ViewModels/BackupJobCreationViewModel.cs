using EasySave.Commands;
using EasySave.Commands.BackupJobs;
using EasySave.Domain.Models;
using EasySave.Models;
using EasySave.Services;
using EasySave.Services.Interfaces;
using EasySave.State.Navigators;
using System.Windows.Input;

namespace EasySave.ViewModels
{
    public class BackupJobCreationViewModel : ViewModelBase
    {
        private readonly IBackupJobService _backupJobService;
        private readonly IRenavigator _renavigator;
        private readonly BackupJobsListingViewModel _backupJobsListingViewModel;

        private BackupJob _backupJob;
        public BackupJob BackupJob
        {
            get
            {
                return _backupJob;
            }
            set
            {
                _backupJob = value;
                OnPropertyChanged(nameof(BackupJob));
            }
        }

        public ICommand CreateBackupJobCommand { get; set; }
        public ICommand ViewBackupJobsCommand { get; set; }

        public BackupJobCreationViewModel(IBackupJobService backupJobService, ILogService logService, IRenavigator backupJobsListingRenavigator, BackupJobsListingViewModel backupJobsListingViewModel)
        {
            _backupJobService = backupJobService;
            _renavigator = backupJobsListingRenavigator;
            _backupJobsListingViewModel = backupJobsListingViewModel;

            BackupJob = new BackupJob(backupJobService, logService, new BackupJobInfo());

            CreateBackupJobCommand = new RelayCommand(CreateBackupJob);

            ViewBackupJobsCommand = new RenavigateCommand(backupJobsListingRenavigator);
        }

        private void CreateBackupJob(object obj)
        {
            _backupJobService.Create(BackupJob);
            _backupJobsListingViewModel.BackupJobs.Add(BackupJob);
            _renavigator.Renavigate();
        }
    }
}
