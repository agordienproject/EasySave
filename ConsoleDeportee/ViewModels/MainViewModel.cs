using ConsoleDeportee.Services;
using EasySave.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConsoleDeportee.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<BackupJobInfo> _backupJobs;
        public ObservableCollection<BackupJobInfo> BackupJobs
        {
            get { return _backupJobs; }
            set
            {
                _backupJobs = value;
                OnPropertyChanged(nameof(BackupJobs));
            }
        }

        private BackupJobInfo _selectedBackupJob;
        public BackupJobInfo SelectedBackupJob
        {
            get { return _selectedBackupJob; }
            set
            {
                _selectedBackupJob = value;
                OnPropertyChanged(nameof(SelectedBackupJob));
            }
        }

        public ICommand ExecuteBackupJobCommand { get; set; }
        public ICommand StopBackupJobExecutionCommand { get; set; }

        public MainViewModel()
        {
            TCPClientManager.BackupJobsListReceived += UpdateBackupJobsList;

            BackupJobs = new ObservableCollection<BackupJobInfo>();
        }

        private void UpdateBackupJobsList(List<BackupJobInfo> backupJobInfos)
        {
            BackupJobs = new ObservableCollection<BackupJobInfo>(backupJobInfos);
        }
    }
}
