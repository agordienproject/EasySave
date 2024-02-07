using EasySave.Enums;
using EasySave.Models;
using EasySave.Services;
using Microsoft.Extensions.Configuration;

namespace EasySave.Controllers
{
    public class BackupController : IBackupController
    {
        private BackupManager _backupManager { get; set; }

        public BackupController(IConfiguration configuration)
        {
            _backupManager = new BackupManager(configuration);
        }

        public async Task CreateBackupJob(string name, string sourcePath, string destinationPath, BackupType backupType)
        {
            await _backupManager.CreateBackupJob(new BackupJob(name, sourcePath, destinationPath, backupType));
        }

        public async Task DeleteBackupJob(string name)
        {
            await _backupManager.DeleteBackupJob(name);
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
