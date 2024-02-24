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
        private readonly ILogService _logService;

        public LoadBackupJobsCommand(BackupJobsListingViewModel backupJobsViewModel, IBackupJobService backupJobService, ILogService logService)
        {
            _backupJobsViewModel = backupJobsViewModel;
            _backupJobService = backupJobService;
            _logService = logService;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _backupJobsViewModel.BackupJobs = new ObservableCollection<BackupJob>();
            foreach (var backupJobInfo in _backupJobService.GetAll())
            {
                if (!_backupJobsViewModel.BackupJobs.Any(backupjob => backupjob.BackupJobId == backupJobInfo.BackupJobId))
                    _backupJobsViewModel.BackupJobs.Add(new BackupJob(_backupJobService, _logService, backupJobInfo));
            }
        }


    }
}
