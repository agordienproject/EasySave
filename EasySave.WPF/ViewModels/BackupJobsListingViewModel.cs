using EasySave.Commands;
using EasySave.Commands.BackupJobs;
using EasySave.Domain.Models;
using EasySave.Models;
using EasySave.Services;
using EasySave.Services.Interfaces;
using EasySave.State.Navigators;
using EasySave.WPF.Utils;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

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

        public ICommand StopBackupJobExecutionCommand { get; set; }

        private Thread BusinessAppListenerThread { get; set; }
        private Thread MaxMemoryListenerThread { get; set; }

        public Dispatcher Dispatcher { get; set; }

        public BackupJobsListingViewModel(IBackupJobService backupJobService, ILogService logService,IRenavigator backupJobCreationRenavigator)
        {
            BackupJobs = new ObservableCollection<BackupJob>();
            BackupJobs.CollectionChanged += BackupJobs_CollectionChanged;

            Dispatcher = Dispatcher.CurrentDispatcher;

            LoadBackupJobsCommand = new LoadBackupJobsCommand(this, backupJobService, logService);
            UpdateBackupJobCommand = new UpdateBackupJobCommand(this, backupJobService);
            DeleteBackupJobCommand = new DeleteBackupJobCommand(this, backupJobService);
            ExecuteBackupJobCommand = new ExecuteBackupJobCommand(this, backupJobService);
            ExecuteBackupJobsCommand = new ExecuteBackupJobsCommand(this, backupJobService);
            
            CreateBackupJobCommand = new RenavigateCommand(backupJobCreationRenavigator);

            StopBackupJobExecutionCommand = new RelayCommand(StopBackupJobExecution);

            BusinessAppListenerThread = new Thread(ListenForBusinessApp);
            BusinessAppListenerThread.IsBackground = true;
            BusinessAppListenerThread.Start();

            MaxMemoryListenerThread = new Thread(ListenForMemoryUsage);
            MaxMemoryListenerThread.IsBackground = true;
            MaxMemoryListenerThread.Start();

            LoadBackupJobsCommand.Execute(this);

            TCPServerManager.NewClientConnection += BroadCastBackupJobs;
            TCPServerManager.ExecuteBackupJobEvent += Deported_ExecuteBackupJob;
            TCPServerManager.StopBackupJobExecutionEvent += Deported_StopBackupJobExecution;
        }

        private void Deported_StopBackupJobExecution(Guid guid)
        {
            BackupJob backupJob = BackupJobs.FirstOrDefault(backupjob => backupjob.BackupJobId == guid);
            if (backupJob != null)
                StopBackupJobExecution(backupJob);
        }

        private void Deported_ExecuteBackupJob(Guid guid)
        {
            BackupJob backupJob = BackupJobs.FirstOrDefault(backupjob => backupjob.BackupJobId == guid);
            if (backupJob != null)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    ExecuteBackupJobCommand.Execute(backupJob);
                }));
            }
        }

        private void BroadCastBackupJobs()
        {
            TCPServerManager.BroadCast(_backupJobs.Select(backupJob => new BackupJobInfo
            {
                BackupJobId = backupJob.BackupJobId,
                BackupName = backupJob.BackupName,
                SourceDirectory = backupJob.SourceDirectory,
                TargetDirectory = backupJob.TargetDirectory,
                BackupType = backupJob.BackupType,
                BackupState = backupJob.BackupState,
                BackupTime = backupJob.BackupTime,
                TotalFilesNumber = backupJob.TotalFilesNumber,
                TotalFilesSize = backupJob.TotalFilesSize,
                FilesSizeLeftToDo = backupJob.FilesSizeLeftToDo,
                NbFilesLeftToDo = backupJob.NbFilesLeftToDo,
                SourceTransferingFilePath = backupJob.SourceTransferingFilePath,
                TargetTransferingFilePath = backupJob.TargetTransferingFilePath,
            }).ToList());
        }

        private void BackupJobs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            BroadCastBackupJobs();
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                    item.PropertyChanged -= OnBackupJob_ItemChanged;
            }
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                    item.PropertyChanged += OnBackupJob_ItemChanged;
            }
        }

        private void OnBackupJob_ItemChanged(object? sender, PropertyChangedEventArgs e)
        {
            BroadCastBackupJobs();
        }

        private void StopBackupJobExecution(object parameter)
        {
            BackupJob backupJob = BackupJobs.First(backupJob => backupJob == (BackupJob)parameter);

            if (backupJob.IsPaused)
            {
                backupJob.Resume();
                backupJob.Stop();
            } else if (backupJob.IsRunning)
            {
                backupJob.Stop();
            }
        }

        private void ListenForBusinessApp()
        {
            bool HasBusinessAppBeenDetected = false;
            while (true)
            {
                if (BusinessAppChecker.IsBusinessAppRunning(Properties.Settings.Default.BusinessAppName))
                {
                    HasBusinessAppBeenDetected = true;
                    foreach (var backupJob in BackupJobs)
                    {
                        if (backupJob.IsRunning)
                        {
                            backupJob.Pause();
                        }
                    }
                }
                else if (HasBusinessAppBeenDetected)
                {
                    foreach (var backupJob in BackupJobs)
                    {
                        if (backupJob.IsPaused)
                        {
                            backupJob.Resume();
                        }
                    }
                    HasBusinessAppBeenDetected = false;
                }
            }
        }
        private void ListenForMemoryUsage()
        {
            bool HasMaxMemoryBeenReached = false;
            while (true)
            {
                if (MemoryUsed.IsMaxMemoryReached(Properties.Settings.Default.MaxMemory))
                {
                    //MessageBox.Show(Resources.Localization.MaxRAMIsReached, "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }


    }
}
