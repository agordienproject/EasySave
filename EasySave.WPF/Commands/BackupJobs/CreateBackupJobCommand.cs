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

        public CreateBackupJobCommand(BackupJobCreationViewModel backupJobCreationViewModel, IBackupJobService backupJobService, IRenavigator backupJobsListingRenavigator)
        {
            _backupJobCreationViewModel = backupJobCreationViewModel;
            _backupJobService = backupJobService;
            _renavigator = backupJobsListingRenavigator;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            await _backupJobService.Create(_backupJobCreationViewModel.BackupJob);

            _renavigator.Renavigate();
        }

    }
}
