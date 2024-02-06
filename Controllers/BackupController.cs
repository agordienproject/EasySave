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

        public void CreateBackupJob(string name, string sourcePath, string destinationPath, BackupType backupType)
        {
            backupManager.CreateBackupJob(new BackupJob(name, sourcePath, destinationPath, backupType));
        }

        public void DeleteBackupJob(string name)
        {
            backupManager.DeleteBackupJob(name);
        }

        public void ShowBackupJobs()
        {
            Console.WriteLine("Displaying...");
            backupManager.DisplayBackupJobs();
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
