using EasySave.Enums;
using EasySave.Models;
using EasySave.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Controllers
{
    public class Controller
    {
        private BackupManager backupManager { get; set; }
        private BackupLogger backupLogger { get; set; }
        
        public Controller() 
        {
            backupManager = new BackupManager();
            backupLogger = new BackupLogger();
        }

        public async Task CreateBackupJob(string name, string sourcePath, string destinationPath, BackupType backupType)
        {
            BackupJob newBackupJob = new BackupJob(name, sourcePath, destinationPath, backupType);
            await backupManager.CreateBackupJob(newBackupJob);
        }

        public async Task DeleteBackupJob(string name)
        {
            List<BackupJob> backupJobs = await backupManager.GetBackupJobs();

            BackupJob backupJobToDelete = backupJobs.Find(backupJob => backupJob.BackupName == name);

            if (backupJobToDelete != null)
            {
                await backupManager.DeleteBackupJob(backupJobToDelete);
            } else
            {
                Console.WriteLine($"No backupJob named : {name} was found !");
            }
        }

        public async Task ShowBackupJobs()
        {
            await backupManager.DisplayBackupJobs();
        }

        public async Task ExecuteBackupJobs(List<int> backupJobsId)
        {
            List<BackupJob> backupJobs = await backupManager.GetBackupJobs();

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
