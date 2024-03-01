using EasySave.Models;
using EasySave.Services.Interfaces;
using EasySave.ViewModels;

namespace EasySave.Commands.BackupJobs
{
    public class DeleteBackupJobCommand : AsyncCommandBase
    {
        private readonly BackupJobsListingViewModel _backupJobsViewModel;
        private readonly IBackupJobService _backupJobService;

        public DeleteBackupJobCommand(BackupJobsListingViewModel backupJobsViewModel, IBackupJobService backupJobService)
        {
            _backupJobsViewModel = backupJobsViewModel;
            _backupJobService = backupJobService;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            BackupJob backupJob = (BackupJob)parameter;

            if (backupJob != null)
            {
                _backupJobService.Delete(backupJob.BackupJobId);
                _backupJobsViewModel.BackupJobs.Remove(backupJob);
            }

        }

    }
}
