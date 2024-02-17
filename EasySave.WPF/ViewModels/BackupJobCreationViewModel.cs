using EasySave.Commands;
using EasySave.Commands.BackupJobs;
using EasySave.Models;
using EasySave.Services.Interfaces;
using EasySave.State.Navigators;
using System.Windows.Input;

namespace EasySave.ViewModels
{
    public class BackupJobCreationViewModel : ViewModelBase
    {
        private readonly IBackupJobService _backupJobService;

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

        public BackupJobCreationViewModel(IBackupJobService backupJobService, IRenavigator backupJobsListingRenavigator, ILogService logService)
        {
            _backupJobService = backupJobService;

            this.BackupJob = new BackupJob(backupJobService, logService);

            CreateBackupJobCommand = new CreateBackupJobCommand(this, backupJobService, backupJobsListingRenavigator);

            ViewBackupJobsCommand = new RenavigateCommand(backupJobsListingRenavigator);
        }
    }
}
