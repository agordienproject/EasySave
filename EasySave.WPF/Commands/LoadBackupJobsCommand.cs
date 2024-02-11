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

        public LoadBackupJobsCommand(BackupJobsViewModel backupJobsViewModel) 
        {
            _backupJobsViewModel = backupJobsViewModel;
        }

        public override Task ExecuteAsync(object parameter)
        {
            throw new NotImplementedException();
        }


    }
}
