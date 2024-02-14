using EasySave.Domain.Models;
using EasySave.Domain.Services;
using EasySave.WPF.Commands;
using EasySave.WPF.State.Navigators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasySave.WPF.ViewModels
{
    public class BackupJobCreationViewModel : ViewModelBase
    {
        private readonly IBackupJobService _backupJobService;

        private BackupJob _backupJob;
        public BackupJob BackupJob
        {
            get
            {
                return _backupJob;
            }
            set
            {
                _backupJob = value;
                OnPropertyChanged(nameof(BackupJob));
            }
        }

        public ICommand CreateBackupJobCommand { get; set; }
        public ICommand ViewBackupJobsCommand { get; set; }

        public BackupJobCreationViewModel(IBackupJobService backupJobService, IRenavigator backupJobsListingRenavigator)
        {
            _backupJobService = backupJobService;

            BackupJob = new BackupJob();

            CreateBackupJobCommand = new CreateBackupJobCommand(this, backupJobService, backupJobsListingRenavigator);

            ViewBackupJobsCommand = new RenavigateCommand(backupJobsListingRenavigator);
        }
    }
}
