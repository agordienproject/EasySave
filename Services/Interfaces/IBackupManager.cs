using EasySave.Enums;
using EasySave.Models;

namespace EasySave.Services.Interfaces
{
    public interface IBackupManager
    {
        Task CopyFiles(string backupJobName, string sourceDir, string targetDir, BackupType backupType);
        Task CreateBackupJob(BackupJob backupJob);
        Task DeleteBackupJob(string backupJobName);
        Task DisplayBackupJobs();
        Task ExecuteBackupJob(BackupJob backupJob);
        Task ExecuteBackupJobs(List<int> backupJobsIndex);
        Task ReadBackups();
        Task WriteBackups();
    }
}