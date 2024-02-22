using EasySave.Models;
using EasySave.Services.Interfaces;
using EasySave.ViewModels;
using EasySave.WPF.Utils;

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
            if (BusinessAppChecker.IsBusinessAppRunning(Properties.Settings.Default.BusinessAppName))
                return;

            BackupJob backupJob = _backupJobsViewModel.BackupJobs.First(backupJob => backupJob == (BackupJob)parameter);

            Thread thread = new Thread(() => backupJob.Execute());
            thread.Start();

        }

    }
}
