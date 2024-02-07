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

        public void CreateBackupJob(string name, string sourcePath, string destinationPath, BackupType backupType)
        {
            _backupManager.CreateBackupJob(new BackupJob(name, sourcePath, destinationPath, backupType));
        }

        public void DeleteBackupJob(string name)
        {
            _backupManager.DeleteBackupJob(name);
        }

        public void ShowBackupJobs()
        {
            _backupManager.DisplayBackupJobs();
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
