using EasySave.Domain.Models;
using EasySave.Domain.Services;
using EasySave.WPF.State.Navigators;
using EasySave.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.WPF.Commands
{
    public class CreateBackupJobCommand : AsyncCommandBase
    {
        private readonly BackupJobCreationViewModel _backupJobCreationViewModel;
        private readonly IBackupJobService _backupJobService;
        private readonly IRenavigator _renavigator;
        private readonly IStateService _stateService;
        
        public CreateBackupJobCommand(BackupJobCreationViewModel backupJobCreationViewModel, IBackupJobService backupJobService, IStateService stateService, IRenavigator backupJobsListingRenavigator)
        {
            _backupJobCreationViewModel = backupJobCreationViewModel;
            _backupJobService = backupJobService;
            _renavigator = backupJobsListingRenavigator;
            _stateService = stateService;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            await _backupJobService.Create(_backupJobCreationViewModel.BackupJob);
            await _stateService.Create(new Domain.Models.State(_backupJobCreationViewModel.BackupJob.BackupName));
            _renavigator.Renavigate();
        }

    }
}
