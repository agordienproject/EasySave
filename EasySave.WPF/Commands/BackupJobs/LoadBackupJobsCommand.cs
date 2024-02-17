using EasySave.Models;
using EasySave.Services.Interfaces;
using EasySave.ViewModels;
using System.Collections.ObjectModel;

namespace EasySave.Commands.BackupJobs
{
    public class LoadBackupJobsCommand : AsyncCommandBase
    {
        private readonly BackupJobsListingViewModel _backupJobsViewModel;
        private readonly IBackupJobService _backupJobService;

        public LoadBackupJobsCommand(BackupJobsListingViewModel backupJobsViewModel, IBackupJobService backupJobService)
        {
            _backupJobsViewModel = backupJobsViewModel;
            _backupJobService = backupJobService;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _backupJobsViewModel.BackupJobs = new ObservableCollection<BackupJob>(await _backupJobService.GetAll());
        }


    }
}
