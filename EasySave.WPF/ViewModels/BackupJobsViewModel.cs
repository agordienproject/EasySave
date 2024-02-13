using EasySave.Domain.Models;
using EasySave.Domain.Services;
using EasySave.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasySave.WPF.ViewModels
{
    public class BackupJobsViewModel : ViewModelBase
    {
        private readonly IBackupJobService _backupJobService;

        private List<BackupJob> _backupJobs;
        public List<BackupJob> BackupJobs 
        { 
            get
            {
                return _backupJobs;
            } 
            set 
            { 
                _backupJobs = value;
                OnPropertyChanged(nameof(BackupJobs));
            }
        }

        public ICommand LoadBackupJobsCommand { get; set; }

        public BackupJobsViewModel(IBackupJobService backupJobService)
        {
            _backupJobService = backupJobService;

            BackupJobs = new List<BackupJob>();

            LoadBackupJobsCommand = new LoadBackupJobsCommand(this);
        }


    }
}
