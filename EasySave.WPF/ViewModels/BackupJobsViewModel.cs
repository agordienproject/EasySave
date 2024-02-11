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
        private readonly BackupJobService _backupJobService;

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

        public BackupJobsViewModel()
        {
            _backupJobService = new BackupJobService();

            BackupJobs = new List<BackupJob>();

            LoadBackupJobsCommand = new LoadBackupJobsCommand(this);
        }


    }
}
