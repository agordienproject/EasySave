using EasySave.Domain.Models;
using EasySave.Domain.Services;
using EasySave.WPF.Commands;
using EasySave.WPF.Commands.BackupJobs;
using EasySave.WPF.State.Navigators;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EasySave.WPF.ViewModels
{
    public class BackupJobsListingViewModel : ViewModelBase
    {
        private List<BackupJob> _backupJobs;
        public List<BackupJob> BackupJobs 
        { 
            get { return _backupJobs; } 
            set 
            { 
                _backupJobs = value;
                OnPropertyChanged(nameof(BackupJobs));
            }
        }

        private BackupJob _selectedBackupJob;
        public BackupJob SelectedBackupJob
        {
            get { return _selectedBackupJob; }
            set
            {
                _selectedBackupJob = value;
                OnPropertyChanged(nameof(SelectedBackupJob));
            }
        }

        public ICommand LoadBackupJobsCommand { get; set; }
        public ICommand UpdateBackupJobCommand { get; set; }
        public ICommand DeleteBackupJobCommand { get; set; }

        public ICommand CreateBackupJobCommand { get; set; }
        public ICommand ExecuteBackupJobCommand { get; set; }

        public BackupJobsListingViewModel(IBackupJobService backupJobService, IStateService stateService, IRenavigator backupJobCreationRenavigator)
        {
            BackupJobs = new List<BackupJob>();

            LoadBackupJobsCommand = new LoadBackupJobsCommand(this, backupJobService);
            UpdateBackupJobCommand = new UpdateBackupJobCommand(this, backupJobService);
            DeleteBackupJobCommand = new DeleteBackupJobCommand(this, backupJobService, stateService);
            ExecuteBackupJobCommand = new ExecuteBackupJobCommand(this, backupJobService);

            CreateBackupJobCommand = new RenavigateCommand(backupJobCreationRenavigator);

            LoadBackupJobsCommand.Execute(this);
        }

    }
}
