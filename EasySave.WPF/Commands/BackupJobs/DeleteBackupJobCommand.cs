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
        private readonly IStateService _stateService;

        public DeleteBackupJobCommand(BackupJobsListingViewModel backupJobsViewModel, IBackupJobService backupJobService, IStateService stateService)
        {
            _backupJobsViewModel = backupJobsViewModel;
            _backupJobService = backupJobService;
            _stateService = stateService;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            await _backupJobService.Delete(_backupJobsViewModel.SelectedBackupJob.BackupName);
            await _stateService.Delete(_backupJobsViewModel.SelectedBackupJob.BackupName);
            _backupJobsViewModel.LoadBackupJobsCommand.Execute(parameter);
        }

    }
}
