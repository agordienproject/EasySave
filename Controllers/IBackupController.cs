using EasySave.Enums;

namespace EasySave.Controllers
{
    public interface IBackupController
    {
        Task CreateBackupJob(string name, string sourcePath, string destinationPath, BackupType backupType);
        Task DeleteBackupJob(string name);
        Task ExecuteBackupJobs(List<int> backupJobsIndex);
        Task ShowBackupJobs();
    }
}