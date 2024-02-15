using EasySave.Domain.Models;
using EasySave.Domain.Services;
using EasySave.WPF.Commands;
using EasySave.WPF.Commands.BackupJobs;
using EasySave.WPF.State.Navigators;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;

namespace EasySave.WPF.ViewModels
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

        //private BackupJob _selectedBackupJob;
        //public BackupJob SelectedBackupJob
        //{
        //    get { return _selectedBackupJob; }
        //    set
        //    {
        //        _selectedBackupJob = value;
        //        OnPropertyChanged(nameof(SelectedBackupJob));
        //    }
        //}

        public ICommand LoadBackupJobsCommand { get; set; }
        public ICommand UpdateBackupJobCommand { get; set; }
        public ICommand DeleteBackupJobCommand { get; set; }

        public ICommand CreateBackupJobCommand { get; set; }
        public ICommand ExecuteBackupJobCommand { get; set; }

        public BackupJobsListingViewModel(IBackupJobService backupJobService, IRenavigator backupJobCreationRenavigator)
        {
            BackupJobs = new ObservableCollection<BackupJob>();


            LoadBackupJobsCommand = new LoadBackupJobsCommand(this, backupJobService);
            UpdateBackupJobCommand = new UpdateBackupJobCommand(this, backupJobService);
            DeleteBackupJobCommand = new DeleteBackupJobCommand(this, backupJobService);
            ExecuteBackupJobCommand = new ExecuteBackupJobCommand(this, backupJobService);

            CreateBackupJobCommand = new RenavigateCommand(backupJobCreationRenavigator);

            //BackupJobs.CollectionChanged += items_CollectionChanged;
            
            LoadBackupJobsCommand.Execute(this);
        }


        //private void items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.OldItems != null)
        //    {
        //        foreach (INotifyPropertyChanged item in e.OldItems)
        //            item.PropertyChanged -= item_PropertyChanged;
        //    }
        //    if (e.NewItems != null)
        //    {
        //        foreach (INotifyPropertyChanged item in e.NewItems)
        //            item.PropertyChanged += item_PropertyChanged;
        //    }
        //}

        //private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    LoadBackupJobsCommand.Execute(this);
        //}
    }
}
