using EasySave.Enums;

namespace EasySave.Controllers
{
    public interface IBackupController
    {
        void CreateBackupJob(string name, string sourcePath, string destinationPath, BackupType backupType);
        void DeleteBackupJob(string name);
        Task ExecuteBackupJobs(List<int> backupJobsId);
        void ShowBackupJobs();
    }
}