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

        public async Task CreateBackupJob(string name, string sourcePath, string destinationPath, BackupType backupType)
        {
            await _backupManager.CreateBackupJob(new BackupJob(name, sourcePath, destinationPath, backupType));
            await _stateManager.CreateState(new State(name));
        }

        public async Task DeleteBackupJob(string name)
        {
            await _backupManager.DeleteBackupJob(name);
            await _stateManager.DeleteState(name);
        }

        public async Task ShowBackupJobs()
        {
            await _backupManager.DisplayBackupJobs();
        }

        public async Task ExecuteBackupJobs(List<int> backupJobsIndex)
        {
            if (backupJobsIndex.Count == 0)
            {
                return;
            }

            await _backupManager.ExecuteBackupJobs(backupJobsIndex);
        }
    }
}
