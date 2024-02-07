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
            feature/sauv_diff_V2
            await backupManager.CreateBackupJob(new BackupJob(name, sourcePath, destinationPath, backupType));
        }

        public async Task DeleteBackupJob(string name)
        {
            feature/sauv_diff_V2
            await backupManager.DeleteBackupJob(name);
        }

        public async Task ShowBackupJobs()
        {
            feature/sauv_diff_V2
            await backupManager.DisplayBackupJobs();
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
