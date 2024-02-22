using EasySave.Services.Interfaces;
using EasySave.State.Navigators;
using EasySave.ViewModels;

namespace EasySave.Commands.BackupJobs
{
    public class CreateBackupJobCommand : AsyncCommandBase
    {
        private readonly BackupJobCreationViewModel _backupJobCreationViewModel;
        private readonly IBackupJobService _backupJobService;
        private readonly IRenavigator _renavigator;

        public CreateBackupJobCommand(BackupJobCreationViewModel backupJobCreationViewModel, IBackupJobService backupJobService, IRenavigator backupJobsListingRenavigator)
        {
            _backupJobCreationViewModel = backupJobCreationViewModel;
            _backupJobService = backupJobService;
            _renavigator = backupJobsListingRenavigator;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _backupJobService.Create(_backupJobCreationViewModel.BackupJob);

            _renavigator.Renavigate();
        }

    }
}
