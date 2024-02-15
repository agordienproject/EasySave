using EasySave.Domain.Models;
using EasySave.Domain.Services;
using EasySave.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.WPF.Commands
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
            await _backupJobService.Delete(_backupJobsViewModel.BackupJobs.First(backupJob => backupJob.BackupName == (parameter as BackupJob).BackupName).BackupName);

            _backupJobsViewModel.LoadBackupJobsCommand.Execute(parameter);
        }

    }
}
