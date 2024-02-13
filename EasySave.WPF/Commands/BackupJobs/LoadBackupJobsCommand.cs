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
    public class LoadBackupJobsCommand : AsyncCommandBase
    {
        private readonly BackupJobsViewModel _backupJobsViewModel;
        private readonly IBackupJobService _backupJobService;

        public LoadBackupJobsCommand(BackupJobsViewModel backupJobsViewModel, IBackupJobService backupJobService)
        {
            _backupJobsViewModel = backupJobsViewModel;
            _backupJobService = backupJobService;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _backupJobsViewModel.BackupJobs = (List<BackupJob>)await _backupJobService.GetAll();
        }

    }
}
