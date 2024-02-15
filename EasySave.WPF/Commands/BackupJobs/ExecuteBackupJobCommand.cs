using EasySave.Domain.Services;
using EasySave.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.WPF.Commands.BackupJobs
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
            await _backupJobService.ExecuteBackupJob(_backupJobsViewModel.SelectedBackupJob);
        }

    }
}
