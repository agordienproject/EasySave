using EasySave.Models;
using EasySave.Services.Interfaces;
using EasySave.ViewModels;

namespace EasySave.Commands.BackupJobs
{
    public class ExecuteBackupJobCommand : AsyncCommandBase
    {
        private readonly BackupJobsListingViewModel _backupJobsViewModel;
        private readonly IBackupJobService _backupJobService;

        public ExecuteBackupJobCommand(BackupJobsListingViewModel backupJobsViewModel, IBackupJobService backupJobService)
        {
            _backupJobsViewModel = backupJobsViewModel;
            _backupJobService = backupJobService;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            await Task.Run(async () =>
            {
                await _backupJobsViewModel.BackupJobs.First(backupJob => backupJob == (BackupJob)parameter).Execute();

            });

        }

    }
}
