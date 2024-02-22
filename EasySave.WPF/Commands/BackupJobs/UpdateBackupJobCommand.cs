using EasySave.Services.Interfaces;
using EasySave.ViewModels;

namespace EasySave.Commands.BackupJobs
{
    public class UpdateBackupJobCommand : AsyncCommandBase
    {
        private readonly BackupJobsListingViewModel _backupJobsViewModel;
        private readonly IBackupJobService _backupJobService;

        public UpdateBackupJobCommand(BackupJobsListingViewModel backupJobsViewModel, IBackupJobService backupJobService)
        {
            _backupJobsViewModel = backupJobsViewModel;
            _backupJobService = backupJobService;
        }

        public override Task ExecuteAsync(object parameter)
        {
            throw new NotImplementedException();
        }

    }
}
