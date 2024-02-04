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
            List<BackupJob> backupJobs = backupManager.GetBackupJobs();

            BackupJob backupJobToDelete = backupJobs.Find(backupJob => backupJob.BackupName == name);

            if (backupJobToDelete != null)
            {
                backupManager.DeleteBackupJob(backupJobToDelete);
            }
            else
            {
                Console.WriteLine($"No backupJob named : {name} was found !");
            }
        }

        public void ShowBackupJobs()
        {
            backupManager.DisplayBackupJobs();
        }

        public async Task ExecuteBackupJobs(List<int> backupJobsId)
        {
            List<BackupJob> backupJobs = backupManager.GetBackupJobs();

            List<BackupJob> backupJobsToExecute = new List<BackupJob>();

            foreach (var index in backupJobsId)
            {
                if (backupJobs.ElementAt(index) == null)
                {
                    backupJobsToExecute.Add(backupJobs.ElementAt(index));
                }
            }

            if (backupJobsToExecute.Count > 0)
            {
                await backupManager.ExecuteBackupJobs(backupJobsToExecute);
            }
        }
    }
}
