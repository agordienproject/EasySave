using EasySave.Domain.Models;
using EasySave.Domain.Services;
using EasySave.WPF.Commands;
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
        public ICommand CreateBackupJobCommand { get; set; }
        public ICommand UpdateBackupJobCommand { get; set; }
        public ICommand DeleteBackupJobCommand { get; set; }

        public BackupJobsViewModel(IBackupJobService backupJobService)
        {
            _backupJobService = backupJobService;

            BackupJobs = new List<BackupJob>();

            LoadBackupJobsCommand = new LoadBackupJobsCommand(this, backupJobService);
            CreateBackupJobCommand = new CreateBackupJobCommand(this, backupJobService);
            UpdateBackupJobCommand = new UpdateBackupJobCommand(this, backupJobService);
            DeleteBackupJobCommand = new DeleteBackupJobCommand(this, backupJobService);
        }


    }
}
