using EasySave.Enums;
using EasySave.Models;
using EasySave.Services;
using Microsoft.Extensions.Configuration;

namespace EasySave.Controllers
{
    public class BackupController : IBackupController
    {
        private BackupManager backupManager { get; set; }

        public BackupController(IConfiguration configuration)
        {
            backupManager = new BackupManager(configuration);
        }

        public async Task CreateBackupJob(string name, string sourcePath, string destinationPath, BackupType backupType)
        {
            await backupManager.CreateBackupJob(new BackupJob(name, sourcePath, destinationPath, backupType));
        }

        public async Task DeleteBackupJob(string name)
        {
            await backupManager.DeleteBackupJob(name);
        }

        public async Task ShowBackupJobs()
        {
            await backupManager.DisplayBackupJobs();
        }

        public async Task ExecuteBackupJobs(List<int> backupJobsIndex)
        {
            if (backupJobsIndex.Count == 0)
            {
                return;
            }

            await backupManager.ExecuteBackupJobs(backupJobsIndex);
        }
    }
}
