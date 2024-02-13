using EasySave.Enums;
using EasySave.Models;
using EasySave.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EasySave.Controllers
{
    public class BackupController : IBackupController
    {
        private IBackupManager _backupManager { get; set; }
        private IStateManager _stateManager { get; set; }

        public BackupController(IBackupManager backupManager, IStateManager stateManager)
        {
            _backupManager = backupManager;
            _stateManager = stateManager;
        }

        public void CreateBackupJob(string name, string sourcePath, string destinationPath, BackupType backupType)
        {
            _backupManager.CreateBackupJob(new BackupJob(name, sourcePath, destinationPath, backupType));
            _stateManager.CreateState(new State(name));

            ShowBackupJobs();
        }

        public void DeleteBackupJob(string name)
        {
            _backupManager.DeleteBackupJob(name);
            _stateManager.DeleteState(name);

            ShowBackupJobs();
        }

        public void ShowBackupJobs()
        {
            _backupManager.DisplayBackupJobs();
        }

        public void ExecuteBackupJobs(List<int> backupJobsIndex)
        {
            if (backupJobsIndex.Count == 0)
            {
                return;
            }

            _backupManager.ExecuteBackupJobs(backupJobsIndex);
        }
    }
}
