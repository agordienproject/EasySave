using EasySave.Commands;
using EasySave.Commands.BackupJobs;
using EasySave.Models;
using EasySave.Services;
using EasySave.Services.Interfaces;
using EasySave.State.Navigators;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EasySave.ViewModels
{
    public class BackupJobsListingViewModel : ViewModelBase
    {
        private ObservableCollection<BackupJob> _backupJobs;
        public ObservableCollection<BackupJob> BackupJobs 
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

        private int _startRange;
        public int StartRange
        {
            get
            {
                return _startRange;
            }
            set
            {
                _startRange = value;
                OnPropertyChanged(nameof(StartRange));
            }
        }

        private int _endRange;
        public int EndRange
        {
            get
            {
                return _endRange;
            }
            set
            {
                _endRange = value;
                OnPropertyChanged(nameof(EndRange));
            }
        }

        public ICommand LoadBackupJobsCommand { get; set; }
        public ICommand UpdateBackupJobCommand { get; set; }
        public ICommand DeleteBackupJobCommand { get; set; }

        public ICommand CreateBackupJobCommand { get; set; }
        public ICommand ExecuteBackupJobCommand { get; set; }

        public ICommand ExecuteBackupJobsCommand { get; set; }

        public BackupJobsListingViewModel(IBackupJobService backupJobService, ILogService logService,IRenavigator backupJobCreationRenavigator)
        {
            BackupJobs = new ObservableCollection<BackupJob>();

            LoadBackupJobsCommand = new LoadBackupJobsCommand(this, backupJobService, logService);
            UpdateBackupJobCommand = new UpdateBackupJobCommand(this, backupJobService);
            DeleteBackupJobCommand = new DeleteBackupJobCommand(this, backupJobService);
            ExecuteBackupJobCommand = new ExecuteBackupJobCommand(this, backupJobService);

            ExecuteBackupJobsCommand = new ExecuteBackupJobsCommand(this, backupJobService);

            CreateBackupJobCommand = new RenavigateCommand(backupJobCreationRenavigator);
            
            LoadBackupJobsCommand.Execute(this);
        }

        
    }
}
